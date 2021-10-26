import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';
 

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegsiter=new EventEmitter();
  registerForm!:FormGroup;
  validatorsError:[]=[];
  constructor(private _accountService:AccountService,private toaster:ToastrService,
              private fb:FormBuilder,private route:Router) { }

  ngOnInit(): void {
    this.initializaForm();
  }
  initializaForm(){
    this.registerForm=this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, 
        Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required]]
    })
  }

   
  onRegister(){
   debugger
    this._accountService.register(this.registerForm.value).subscribe(res=>{
      debugger
      this.route.navigateByUrl('/member')
     console.log(res)
     this.onCancel();
    },err=>{
      this.validatorsError=err;
    })
     
  }
  onCancel(){
    this.cancelRegsiter.emit(false);
  }

}
