import { Component, OnInit } from '@angular/core';
import { AccountService } from '../Services/account.service';
import { User } from '../_models/user';
 
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  UserLogin:any={};
  constructor(public _accountService:AccountService) { }

  ngOnInit(): void {
  }
  onLogin(){
    this._accountService.login(this.UserLogin).subscribe(res=>{
      alert("You have loggedin")
    },err=>{
      console.log(err);
    })
    //console.log(this.UserLogin.userName, this.UserLogin.password)
  }
  onLogout(){
    alert('hh')
    this._accountService.logout();
  }
 
}
