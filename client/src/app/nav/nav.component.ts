import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';
import { User } from '../_models/user';
 
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  UserLogin:any={};
  constructor(public _accountService:AccountService,private router:Router,private toaster:ToastrService) { }

  ngOnInit(): void {
   
  }
  onLogin(){
    this._accountService.login(this.UserLogin).subscribe(res=>{
      this.router.navigateByUrl('/member');
    },err=>{
      console.log(err);
      this.toaster.error(err.error);
      
    })
    //console.log(this.UserLogin.userName, this.UserLogin.password)
  }
  onLogout(){
    this._accountService.logout();
    this.router.navigateByUrl('/');
  }
 
}
