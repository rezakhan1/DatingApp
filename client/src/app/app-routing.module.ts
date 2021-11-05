import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorHandlingComponent } from './Error/error-handling/error-handling.component';
import { NotFoundComponent } from './Error/not-found/not-found.component';
import { ServerNotFoundComponent } from './Error/server-not-found/server-not-found.component';
import { AuthdGuard } from './guards/authguard';
import { PreventUnsavedChangesGuard } from './guards/prevent-unsaved-changes.guard';

import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberdetailresolverResolver } from './memberdetailresolver.resolver';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';

const routes: Routes = [
  { path:'', component:HomeComponent },
  {
    path:'',
    canActivate:[AuthdGuard],
    runGuardsAndResolvers:'always',
    children:[
      { path:'member/:username', component:MemberDetailsComponent ,resolve:{member:MemberdetailresolverResolver} },
      { path:'members/edit', component:MemberEditComponent, canDeactivate:[PreventUnsavedChangesGuard] },
      { path:'member', component:MemberListComponent},
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
