import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

//import { User } from '../_Model/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl=environment.apiUrl;
  private currentSource=new ReplaySubject<User>(1);
  public currentUser$=this.currentSource.asObservable();
  constructor(private http:HttpClient) { }
 
    login(userModel:any){
     return this.http.post(this.baseUrl+"account/login",userModel).pipe(map((response: User)=>{
       const user=response;
       if(user){
         debugger;
        localStorage.setItem('user',JSON.stringify(user));
        this.currentSource.next(user);
       }

     }))
    }
    register(userModel:any){
    return  this.http.post(this.baseUrl+"account/register",userModel).pipe(map((user:User)=>{
      if(user){
        localStorage.setItem('user',JSON.stringify(user));
        this.currentSource.next(user);
      }
      return user;
      }))
    }
  setCurrentUser(user:User){
    const User=user?user:undefined;
   this.currentSource.next(User);
  }
    logout(){
      localStorage.removeItem('user');
      this.currentSource.next(undefined);
    }
}
 