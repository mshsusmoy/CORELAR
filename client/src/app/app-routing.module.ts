import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestsErrorsComponent } from './errors/tests-errors/tests-errors.component';
import { FeedViewComponent } from './feed-view/feed-view.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { ChangePasswordComponent } from './members/change-password/change-password/change-password.component';
import { MemberDetailtComponent } from './members/member-detailt/member-detailt.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsaveChangesGuard } from './_guards/prevent-unsave-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-detailed.resolver';

const routes: Routes = [
  {path: '',component: HomeComponent},
  {
    path:'',
    runGuardsAndResolvers: 'always',
    canActivate:[AuthGuard],
    children:[
      {path: 'members',component: MemberListComponent},
      {path: 'members/:username',component: MemberDetailtComponent, resolve: {member: MemberDetailedResolver}},
      {path: 'member/edit',component: MemberEditComponent,canDeactivate:[PreventUnsaveChangesGuard]},
      {path: 'lists',component: ListsComponent},
      {path: 'messages',component: MessagesComponent},
      {path: 'member/change-password',component: ChangePasswordComponent},
      {path: 'feed',component: FeedViewComponent},
      {path: 'admin',component: AdminPanelComponent, canActivate:[AdminGuard]}
    ]
  },
  {path:'errors', component:TestsErrorsComponent},
  {path:'not-found',component:NotFoundComponent},
  {path: 'server-error',component:ServerErrorComponent},
  {path: '**',component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
