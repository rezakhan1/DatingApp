import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../Services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthdGuard implements CanActivate {
  
  constructor(private accountService:AccountService,private toasterService:ToastrService){

  }
  canActivate(): Observable<boolean> {
  return  this.accountService.currentUser$.pipe(
      map(user=>{
        if(user) return true;
        else{
         this.toasterService.error("You are not authorized");
         return false;
        }
      })
    )

  }
  
}
