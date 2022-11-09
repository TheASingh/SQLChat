import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ChatroomComponent } from './chatroom/chatroom.component'
import { HomeComponent } from './home/home.component'
import { RoomlistComponent } from './roomlist/roomlist.component'
import { ChatauthguardGuard } from './chatauthguard.guard';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'roomlist',
    component: RoomlistComponent,
    canActivate: [ChatauthguardGuard]
  },
  //{ path: 'chatroom/:roomname', component: ChatroomComponent },
  {
    path: 'chatroom',
    component: ChatroomComponent,
    canActivate: [ChatauthguardGuard]
  }

  //{
  //  path: '',
  //  redirectTo: '/chatroom',
  //  pathMatch: 'full'
  //}
];



@NgModule({
  declarations: [],
  imports: [
    //RouterModule.forRoot(routes, { useHash: true }),
    RouterModule.forRoot(routes),
    CommonModule
  ]
})
export class AppRoutingModule { }
