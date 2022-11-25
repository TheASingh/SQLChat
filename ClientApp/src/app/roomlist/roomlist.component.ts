import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ChatapplicationServiceService } from '../chatapplicationservice.service';
import { HttpClient, HttpParams } from '@angular/common/http';


var snackBar: MatSnackBar;

@Component({
  selector: 'app-roomlist',
  templateUrl: './roomlist.component.html',
  styleUrls: ['./roomlist.component.css']
})
export class RoomlistComponent implements OnInit {

  loggedInUserEmail: string = '';
  roomListToDisplay: any = [];
  displayedColumns: string[] = ['roomName'];
  userId: any = null;
  showRoomListProgressBar: boolean= true;

  constructor(private router: Router, private http: HttpClient, private authService: ChatapplicationServiceService, _snackBar: MatSnackBar) {

    snackBar = _snackBar;
   
    if (localStorage.getItem('CurrentUser')) {
      this.userId = JSON.parse(localStorage.CurrentUser).userId;
    }
  }

  ngOnInit(): void {
    
    setTimeout(() => this.getRoomList(), 500);

    // using ! operator to let compiler know that the object is not undefined
    this.authService.hubConnection!.on("SendToRoomListComponent", (data) => {
      
      this.getRoomList();
    });
   
  }


  getRoomList() {
    
    var urlToGetRooms = this.authService.baseApiUrl + this.authService.getRooms_endpoint;
    let parameters = new HttpParams().set("userId", this.userId);
    this.http.get<any>(urlToGetRooms, { params: parameters }).subscribe(res => {
      this.showRoomListProgressBar = false;
      this.roomListToDisplay = res.data;
    })

  }

  addRoom(roomListForm: any) {


  }

  enterChatRoom(roomId: any) {
    // To pass params, also need to mention the paramname in Routes in app-routing.module.ts
    //this.router.navigate(['chatroom', roomname]);

    // To prevent the roomname to get appended to the url, here, assigning the roomname parameter to service-class parameter.
    this.authService.chatRoomToOpen = roomId;
    this.router.navigate(['chatroom']);
  }

  onLogOut() {
    this.authService.logout();
  }


}
