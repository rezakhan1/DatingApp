import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/Services/account.service';
import { MessageService } from 'src/app/Services/message.service';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';

@Component({
  changeDetection:ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
 @Input() messages: Message[];
 @Input() username:string;
 user:User;
  messageContent: string;
  loading = false;
  constructor(public messageService:MessageService,private accout:AccountService) { 
    this.accout.currentUser$.pipe(take(1)).subscribe(res=>{
      this.user=res;
       
     })
  }

  ngOnInit(): void {
   
  }

 sendMessage(){
   this.messageService.sendMessage(this.username,this.messageContent).then(()=>{
    //this.messages.push(message);
   this.messageForm.reset();
   })
 }
getLocation(message:any):string{
  debugger;
  return (message.senderUsername) === this.user.userName?"float: left":"float:right";
}
getLocationmssg(message):string{
  debugger
  return (message.senderUsername ) === this.user.userName?"margin-left: 69%;":"";
}
}
