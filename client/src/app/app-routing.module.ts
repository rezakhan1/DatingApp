import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorHandlingComponent } from './Error/error-handling/error-handling.component';
import { NotFoundComponent } from './Error/not-found/not-found.component';
import { ServerNotFoundComponent } from './Error/server-not-found/server-not-found.component';
import { AuthdGuard } from './guards/authguard';

import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';

const routes: Routes = [
  { path:'', component:HomeComponent },
  {
    path:'',
    canActivate:[AuthdGuard],
    runGuardsAndResolvers:'always',
    children:[
      { path:'member/:id', component:MemberDetailsComponent },
      { path:'member', component:MemberListComponent,canActivate:[AuthdGuard] },
      { path:'lists', component:ListsComponent },
      { path:'message', component:MessagesComponent },
    ]
  },
  {path:'not-found',component:NotFoundComponent},
  {path:'server-found',component:ServerNotFoundComponent},
  {path:'buggy', component:ErrorHandlingComponent},
  { path:'**', component:NotFoundComponent, pathMatch:'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
