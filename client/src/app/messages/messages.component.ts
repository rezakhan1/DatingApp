import { Component, OnInit } from '@angular/core';
import { MessageService } from '../Services/message.service';
import { ConfirmService } from '../_models/confirm.service';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[] = [];
  pagination: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  loading = false;
  constructor(private messageervice:MessageService,private confirmService: ConfirmService) { }

  ngOnInit(): void {
    this.loadMessages();
  }
  loadMessages(){
     this.loading=true;
     this.messageervice.getMessages(this.pageNumber,this.pageSize,this.container).subscribe(res=>{
       debugger;
       this.messages=res.result;
       this.pagination=res.pagination;
       this.loading=false;
       
     })
   }
   deleteMessage(id: number) {
    this.confirmService.confirm('Confirm delete message', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.messageervice.deleteMessage(id).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        })
      }
    })

  }
   pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadMessages()
  }
}
