import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component'
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MembersComponent } from './members/members.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import {ToastrModule} from 'ngx-toastr'
import { SharedModule } from '_modules/shared.module';
import { ErrorHandlingComponent } from './Error/error-handling/error-handling.component';
import { ErrorInterceptor } from './interceptor/error.interceptor';
import { NotFoundComponent } from './Error/not-found/not-found.component';
import { ServerNotFoundComponent } from './Error/server-not-found/server-not-found.component';
import { MemberCardsComponent } from './members/member-cards/member-cards.component';
import { JwtInterceptor } from './interceptor/jwt.interceptor';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { NgxSpinner, NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './interceptor/loading.interceptor';
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MembersComponent,
    MemberListComponent,
    MemberDetailsComponent,
    ListsComponent,
    MessagesComponent,
    ErrorHandlingComponent,
    NotFoundComponent,
    ServerNotFoundComponent,
    MemberCardsComponent,
    MemberEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    NgxGalleryModule,
    NgxSpinnerModule
   
  ],
  providers: [
    { provide:HTTP_INTERCEPTORS,useClass:ErrorInterceptor,multi:true},
    { provide:HTTP_INTERCEPTORS,useClass:JwtInterceptor,multi:true},
    { provide:HTTP_INTERCEPTORS,useClass:LoadingInterceptor,multi:true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
