import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { MemberService } from './Services/member.service';
import { Member } from './_models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberdetailresolverResolver implements Resolve<Member> {
  constructor(private memberService:MemberService){}
  resolve(route: ActivatedRouteSnapshot): Observable<Member> {
   return this.memberService.getMember(route.paramMap.get('username'));
  }
}
