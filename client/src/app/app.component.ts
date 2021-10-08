import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './Services/account.service';
import { User } from './_models/user'; 
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users:any;
  constructor(private http:HttpClient,private _service:AccountService){

  }
  ngOnInit() {
   this.getUsers();
   this.setCurrentUser();
  }
  setCurrentUser(){
    debugger;
    const user:User =JSON.parse(localStorage.getItem('user')!);
    this._service.setCurrentUser((user));
   }
  getUsers(){
    this.http.get("https://localhost:5001/api/user/allusers").subscribe(result=>{
      this.users=result
    } ,err=>{
      console.log(err)
    })
  }
}
