import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './Services/account.service';
import { PresenceService } from './Services/presence.service';
import { User } from './_models/user'; 
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users:any;
  constructor(private http:HttpClient,private _service:AccountService,private presence: PresenceService){

  }
  ngOnInit() {
   this.setCurrentUser();
  }
  setCurrentUser(){
   
    const user:User =JSON.parse(localStorage.getItem('user')!);
    if (user) {
      this._service.setCurrentUser(user);
      this.presence.createHubConnection(user);
    }
   }
}
