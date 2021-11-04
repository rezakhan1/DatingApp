import { Component, OnInit } from '@angular/core';
import { MemberService } from '../Services/member.service';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members:Partial<Member[]>;
  predicate='liked'
  pageNumber = 1;
  pageSize = 1;
  pagination: Pagination;
  constructor(private _memberservice:MemberService) { }

  ngOnInit(): void {
    this.onLoadLike();
  }
   onLoadLike(){
     this._memberservice.getLikes(this.predicate,this.pageNumber,this.pageSize).subscribe(res=>{
      this.members = res.result;
      this.pagination = res.pagination;
     
     })
   }
   pageChanged(event: any) {
    this.pageNumber = event.page;
    this.onLoadLike();
  }
}
