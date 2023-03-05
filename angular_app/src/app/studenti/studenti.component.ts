import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Router} from "@angular/router";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css']
})
export class StudentiComponent implements OnInit {

  title:string = 'angularFIT2';
  ime_prezime:string = '';
  opstina: string = '';
  studentPodaci: any;
  filter_ime_prezime: boolean;
  filter_opstina: boolean;
  odabrani_student:any;


  constructor(private httpKlijent: HttpClient, private router: Router) {
  }

  fetchStudenti() :void
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Student/GetAll", MojConfig.http_opcije()).subscribe(x=>{
      this.studentPodaci = x;
    });
  }

  ngOnInit(): void {
    this.fetchStudenti();
  }

  getPodaci(){
    if (this.studentPodaci==null)
      return [];
    else if(this.filter_opstina || this.filter_ime_prezime){
    return this.studentPodaci.filter((s:any)=>
      ((!this.filter_ime_prezime) ||
      (s.ime+ " " + s.prezime).startsWith(this.ime_prezime) ||
      (s.prezime+ " " + s.ime).startsWith(this.ime_prezime)) &&
      ((!this.filter_opstina) ||
        (s.opstina_rodjenja!=null && s.opstina_rodjenja.description).startsWith(this.opstina)
      )
    );}
    else{
      return this.studentPodaci;
    }

  }

  Obrisi(s: any) {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/Student/Obrisi", s ,MojConfig.http_opcije()).subscribe(x=>{
      porukaSuccess("Uspjesno brisanje studenta");
      this.fetchStudenti();
    });
  }

  Uredi(s: any) {
    this.odabrani_student=s;
  }

  Maticna(s: any) {
    this.router.navigate(["/student-maticnaknjiga",s.id]);
  }

  Novi() {
    this.odabrani_student={
      id:0,
      opstina_rodjenja_id:10,
      ime: this.ime_prezime,
      prezime: ""
    }
  }
}
