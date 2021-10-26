import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/Services/account.service';
import { MemberService } from 'src/app/Services/member.service';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
 @Input() member!:Member;
 uploader!: FileUploader;
 hasBaseDropzoneOver = false;
 baseUrl = environment.apiUrl;
 user!: User;
  constructor(private _account:AccountService,private accountService:AccountService,
             private memberService:MemberService) {
    _account.currentUser$.pipe(take(1)).subscribe(user=>this.user=user);
   }

  ngOnInit(): void {
    this.initializeUploader();
  }
  
  
  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'user/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.member.photos?.push(photo);
        if(photo.isMain){
          this.user.photoUrl=photo.url;
          this.member.photoUrl=photo.url;
          this._account.setCurrentUser(this.user);
        }
        
      }
    }
}
onSetMainPic(photo: Photo) {
  debugger;
  this.memberService.setMainPhoto(photo.id).subscribe(() => {
    debugger;
    this.user.photoUrl = photo.url;
    this.accountService.setCurrentUser(this.user);
    this.member.photoUrl = photo.url;
    this.member.photos?.forEach(p => {
      if (p.isMain) p.isMain = false;
      if (p.id === photo.id) p.isMain = true;
    })
  })
} 
onDelete(photo: Photo) {
  this.memberService.deletePhoto(photo.id).subscribe(() => {
    this.memberService.getMember(this.user.userName!).subscribe(res=>{
      debugger;
      this.member=res;
    })
    this.member.photos = this.member.photos!.filter(x => x.id !== photo.id);
  })
} 

}
