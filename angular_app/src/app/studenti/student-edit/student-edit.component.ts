import {Component, Input, OnInit} from '@angular/core';
import {MojConfig} from "../../moj-config";
import {HttpClient} from "@angular/common/http";
declare function porukaSuccess(a: string):any;

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.css']
})
export class StudentEditComponent implements OnInit {

  constructor(private httpKlijent: HttpClient) { }
  @Input() student:any;
  opstine_podaci:any;
  ngOnInit(): void {
    this.fetchOpstine()
  }

  fetchOpstine(){
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Opstina/GetByAll", MojConfig.http_opcije()).subscribe(x=>{
      this.opstine_podaci = x;
    });
  }
  Snimi() {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/Student/Snimi", this.student,MojConfig.http_opcije()).subscribe(x=>{
      porukaSuccess("Uspjesno snimanje korisnika");
      location.reload();
    });
  }
}
