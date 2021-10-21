import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { MemberService } from 'src/app/Services/member.service';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  memberList$!:Observable<Member[]>;
  constructor(private _memberService:MemberService) { }

  ngOnInit(): void {
    this.memberList$=this._memberService.getMembers();
  }
}
