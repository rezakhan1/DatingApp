import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, retry } from 'rxjs/operators';
import { Options } from 'selenium-webdriver';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
 
@Injectable({
  providedIn: 'root'
})
export class MemberService {
 baseUrl= environment.apiUrl
 members:Member[]=[];
 paginationResult:PaginatedResult<Member[]>=new PaginatedResult<Member[]>();
 memberCache= new Map();
  constructor(private http:HttpClient) { } 
  
  getMembers(userParams:UserParams){
   var alreadySearched=  this.memberCache.get(Object.values(userParams).join('-'))
   if(alreadySearched){
     return of(alreadySearched);
   }
  let params=getPaginationHeaders(userParams.pageNumber,userParams.pageSize)

  params = params.append('minAge',userParams.minAge.toString())
  params = params.append('maxAge',userParams.maxAge.toString())
  params = params.append('gender',userParams.gender)
  params = params.append('orderBy',userParams.orderBy)

  return getPaginatedResult<Member[]>(this.baseUrl + 'user/allusers', params, this.http).pipe(map(res=>{
     this.memberCache.set(Object.values(userParams).join('-'),res)
    return res;
  }))
    
  }
  
  getMember(username:string){

    const member = [...this.memberCache.values()]
    .reduce((arr, elem) => arr.concat(elem.result), [])
    .find((member: Member) => member.userName === username.toLowerCase());
    console.log(this.memberCache)
    if (member) {
      return of(member);
    }
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

    setMainPhoto(photoId:number){
    return  this.http.post(this.baseUrl+'user/set-photo-main/'+photoId,{});
    }
    deletePhoto(photoId:number){
      return  this.http.delete(this.baseUrl+'user/delete-photo/'+photoId,{});
      }

   addLike(userName:string){
     debugger
    return this.http.post(this.baseUrl+'likes/'+userName.toLowerCase(),{})
   }   
   
   getLikes(predicate:string,pageNumber, pageSize){
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);
    return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params, this.http);
   // return this.http.get<Partial<Member[]>>(this.baseUrl+'likes?predicate='+predicate);

   }
    
}
 
