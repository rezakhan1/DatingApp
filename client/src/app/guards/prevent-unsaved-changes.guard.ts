import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  constructor() {
    //super();
    
  }
  canDeactivate(member:MemberEditComponent):boolean {
      if(member.editMember.dirty){
       return  confirm("Are you want continue?")
      }
    return true;
  }
  
}