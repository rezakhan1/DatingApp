
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MemberService } from 'src/app/Services/member.service';
import { MessageService } from 'src/app/Services/message.service';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
  @ViewChild('memberTabs', {static:true}) memberTabs: TabsetComponent;
  member:Member;
  galleryOptions!: NgxGalleryOptions[];
  galleryImages!: NgxGalleryImage[];
  aciveTab:TabDirective;
  message: Message[]=[];
  constructor(private route:ActivatedRoute,private _memberService:MemberService,
    private messageService:MessageService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.member = data.member;
    })
   this.galleryOptions = [
    {
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }
  ]
  this.route.queryParams.subscribe(param=>{
    param.tab?this.selectTab(param.tab):this.selectTab(0);
   })
   this.galleryImages = this.getImages();
  }

getImages(): NgxGalleryImage[] {
  const imageUrls = [];
  for (const photo of this.member.photos!) {
    imageUrls.push({
      small: photo?.url,
      medium: photo?.url,
      big: photo?.url
    })
  }
  return imageUrls;
}
 onTabActivated(data:TabDirective){
 this.aciveTab=data;
    if(this.aciveTab.heading ==='Messages' && this.message.length===0){
      this.loadMessage();
    }
  }
  loadMessage(){
    debugger
   this.messageService.getMessageThread(this.member.userName).subscribe(res=>{
    this.message=res;
   })
  }
  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  
}
