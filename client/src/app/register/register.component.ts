import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { AccountService } from '../Services/account.service';
 

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerModel:any={};
  @Output() cancelRegsiter=new EventEmitter();
  constructor(private _accountService:AccountService) { }

  ngOnInit(): void {
  }
  onRegister(){
    this._accountService.register(this.registerModel).subscribe(res=>{
     console.log(res)
     this.onCancel();
    },err=>{
      console.log(err);
    })
     
  }
  onCancel(){
    this.cancelRegsiter.emit(false);
  }

}
