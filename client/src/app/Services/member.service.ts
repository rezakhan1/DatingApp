import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
 
@Injectable({
  providedIn: 'root'
})
export class MemberService {
 baseUrl= environment.apiUrl
 members:Member[]=[];
  constructor(private http:HttpClient) { } 
  
  getMembers(){
    if(this.members.length>0) return of(this.members)
  return  this.http.get<Member[]>(this.baseUrl+'user/allusers').pipe(
    map(res=>{
     return this.members=res;
    })
  )
  }

  getMember(username:string){
    debugger;
    const member=this.members.find(x=>x.userName===username.toLocaleLowerCase())
    if(member !== undefined) return of(member);
    return  this.http.get<Member>(this.baseUrl+'user/'+username.toLocaleLowerCase());
    }
    updateMember(member: Member) {
      return this.http.put(this.baseUrl + 'user', member).pipe(
        map(() => {
          const index = this.members.indexOf(member);
          this.members[index] = member;
        })
      )
    }
    
}
 
