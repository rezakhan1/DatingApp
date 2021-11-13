import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MessageService } from 'src/app/Services/message.service';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
 @Input() messages: Message[];
 @Input() username:string;
 
  messageContent: string;
  loading = false;
  constructor(public messageService:MessageService) { }

  ngOnInit(): void {
   
  }

 sendMessage(){
   this.messageService.sendMessage(this.username,this.messageContent).then(()=>{
    //this.messages.push(message);
   this.messageForm.reset();
   })
 }

}
