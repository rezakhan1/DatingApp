import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MemberService } from 'src/app/Services/member.service';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-cards',
  templateUrl: './member-cards.component.html',
  styleUrls: ['./member-cards.component.css']
})
export class MemberCardsComponent implements OnInit {

  @Input()  member:Member;
  
  constructor(private _memberService:MemberService, private toaster:ToastrService) { }

  ngOnInit(): void {
  }
   addLikes(member:Member){
     this._memberService.addLike(member.userName).subscribe((res:Member)=>{
       debugger;
      this.toaster.success('You Have Liked '+member.knownAs)
     })
   }
}
