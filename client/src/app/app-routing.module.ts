import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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
  { path:'**', component:HomeComponent, pathMatch:'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
