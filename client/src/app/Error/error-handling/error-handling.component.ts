import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-error-handling',
  templateUrl: './error-handling.component.html',
  styleUrls: ['./error-handling.component.css']
})
export class ErrorHandlingComponent implements OnInit {
   baseUrl:string=environment.apiUrl// "https://localhost:5001/api/";
   validationError:string[]=[];
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
  }
  get401(){
    this.http.get(this.baseUrl+'Buggy/auth').subscribe(res=>{

      console.log(res);
    }, err=>{
    console.log(err);
    })
  }
  get501(){
    this.http.get(this.baseUrl+'Buggy/server-error').subscribe(res=>{

      console.log(res);
    }, err=>{
    console.log(err);
    })
  }
  get400(){
    this.http.get(this.baseUrl+'Buggy/bad-request').subscribe(res=>{

      console.log(res);
    }, err=>{
    console.log(err);
    })
  }
  get404(){
    this.http.get(this.baseUrl+'Buggy/not-found').subscribe(res=>{

      console.log(res);
    }, err=>{
    console.log(err);
    this.validationError=err;
    })
  }

}
