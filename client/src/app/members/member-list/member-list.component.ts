import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/Services/account.service';
import { MemberService } from 'src/app/Services/member.service';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  memberList:Member[];
  pagination:Pagination;
  userParam:UserParams;
  user:User;
  genderList=[{value:'male',display:'Male'},{value:'female',display:'Female'}]
  constructor(private _memberService:MemberService,private accout:AccountService) { 
     this.accout.currentUser$.pipe(take(1)).subscribe(res=>{
      this.user=res;
      this.userParam=new UserParams(this.user);
     })
  }

  ngOnInit(): void {
    //this.memberList$=this._memberService.getMembers();
    this.loadMember();
  }
  loadMember(){
    debugger;
    this._memberService.getMembers(this.userParam).subscribe(response=>{
      debugger;
    this.memberList=response.result!;
    this.pagination=(response.pagination)
    })
  }
  pageChanged(event:any){
   this.userParam.pageNumber=event.page;
   this.loadMember();
  }

  onresetFilters(){
    this.userParam=new UserParams(this.user);
    this.loadMember();
  }
}
