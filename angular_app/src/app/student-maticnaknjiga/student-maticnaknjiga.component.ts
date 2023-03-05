import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";

declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css']
})
export class StudentMaticnaknjigaComponent implements OnInit {
  constructor(private httpKlijent: HttpClient, private route: ActivatedRoute) {}

  studentid:any;
  godina:any;
  akademske_podaci:any;
  podacistudenta:any;
  ovjerasemestra:any;

  ngOnInit(): void {
    this.route.params.subscribe(s=>{
      this.studentid= +s["id"];
    });
    this.fetchAkademskeGodine();
    this.fetchPodatkeStudenta();
  }

  UpisiGodinu() {
    this.godina={
      id:0,
      datumUpisa:new Date().toISOString().slice(0,10),
      studentid:this.studentid
    }
  }

  fetchAkademskeGodine(){
    this.httpKlijent.get(MojConfig.adresa_servera+ "/AkademskeGodine/GetAll_ForCmb",MojConfig.http_opcije()).subscribe(s=>{
      this.akademske_podaci=s;
    });
}

  DodajGodinu() {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/MaticnaKnjiga/Dodaj",this.godina,MojConfig.http_opcije()).subscribe(s=>{
      porukaSuccess("Uspjesno dodavanje godine");
      this.fetchPodatkeStudenta();
    });
  }

 fetchPodatkeStudenta() {
   this.httpKlijent.get(MojConfig.adresa_servera+ "/MaticnaKnjiga/GetById?id="+ this.studentid,MojConfig.http_opcije()).subscribe(s=>{
     this.podacistudenta=s;
     console.log(this.podacistudenta);
   });
  }

  OvjeriSemestar(p: any) {
    this.ovjerasemestra={
      datumOvjere: new Date().toISOString().slice(0,10),
      id:p.id
    }
  }

  Update() {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/MaticnaKnjiga/Update",this.ovjerasemestra,MojConfig.http_opcije()).subscribe(s=>{
      porukaSuccess("Uspjesna ovjera semestra");
      this.fetchPodatkeStudenta();
      this.ovjerasemestra=null;
    });
  }
}
