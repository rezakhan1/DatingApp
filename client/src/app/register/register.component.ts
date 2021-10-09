import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';
 

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerModel:any={};
  @Output() cancelRegsiter=new EventEmitter();
  constructor(private _accountService:AccountService,private toaster:ToastrService) { }

  ngOnInit(): void {
  }
  onRegister(){
    this._accountService.register(this.registerModel).subscribe(res=>{
     console.log(res)
     this.onCancel();
    },err=>{
      this.toaster.error(err.error);
    })
     
  }
  onCancel(){
    this.cancelRegsiter.emit(false);
  }

}
