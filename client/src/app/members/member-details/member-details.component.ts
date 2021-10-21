import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { MemberService } from 'src/app/Services/member.service';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {

  member!:Member;
  galleryOptions!: NgxGalleryOptions[];
  galleryImages!: NgxGalleryImage[];
  constructor(private route:ActivatedRoute,private _memberService:MemberService) { }

  ngOnInit(): void {
   this.loadMember();
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

  }
loadMember(){
  this._memberService.getMember(this.route.snapshot.paramMap.get('username')!).subscribe(res=>{
    debugger;
    this.member=res;
    this.galleryImages = this.getImages();
  })
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
}
