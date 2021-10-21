import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/Services/account.service';
import { MemberService } from 'src/app/Services/member.service';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user!:User;
  member!:Member;
  @ViewChild('editMember') editMember!:NgForm;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editMember.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private _accoutService:AccountService,private _memberService:MemberService,
             private _toaster:ToastrService) {
    _accoutService.currentUser$.pipe(take(1)).subscribe(res=>{
       this.user=res;
     })
     debugger;
   }

  ngOnInit(): void {
    this.loadMember()
  }
  loadMember(){
    this._memberService.getMember(this.user.userName!).subscribe(res=>{
      this.member=res;
    })
  }
  UpdateMember(){
    this._memberService.updateMember(this.member).subscribe(()=>{
      this._toaster.success("Member Updated");
      this.editMember.reset(this.member);
    })

  }
}
