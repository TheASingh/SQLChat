import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit, ElementRef, ViewChild, AfterViewChecked, OnDestroy, AfterContentInit, AfterViewInit, AfterContentChecked } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { ChatapplicationServiceService } from '../chatapplicationservice.service';


@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit, OnDestroy, AfterContentInit, AfterViewInit {

  loggedInUserName: string = ''
  loggedInUserId = 0;;
  month = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
  chatDataToDisplayList: any = [];
  selectedRoomId: string = '';
  numberOfUnseenMessages: number = 0;

  //private hubConnection: HubConnection | undefined;


  constructor(private router: Router,
    private http: HttpClient,
    private route: ActivatedRoute,
    private authService: ChatapplicationServiceService) {

    // To get the params passed through router.Navigate() method.
    // But it has a drawback as it appends the parameter name to the url.
    // Lets, if room3 is not created yet, if user appends 'room3' to the url, it will pass 'room3' as the parameter.

    //this.selectedRoomName = this.route.snapshot.params.roomname;



    if (this.authService.chatRoomToOpen) {
      this.selectedRoomId = this.authService.chatRoomToOpen;
    }
    else {
      this.router.navigate(['roomlist']);
    }


    if (!localStorage.CurrentUser) {
      console.log("No active logged in user")
      return;
    }
    else {
      this.loggedInUserName = JSON.parse(localStorage.CurrentUser).userName;
      this.loggedInUserId = JSON.parse(localStorage.CurrentUser).userId;
    }
  }
  //ngAfterContentChecked(): void {
  //  var ui = 5;
  //  var it = document.getElementById('wave12');
  //  console.log("ngAfterContentChecked", it);
  //  var uii = 4;
  //}
  //ngAfterViewChecked(): void {
  //  var ui = 5;
  //  var it = document.getElementById('wave12');
  //  console.log("ngAfterViewChecked", it);

  //  var uii = 4;
  //}

  ngAfterViewInit(): void {


  }


  ngAfterContentInit(): void {

  }



  @ViewChild('chatcontent', { static: false }) chatcontent: ElementRef<HTMLInputElement> = {} as ElementRef;
  @ViewChild('chatform', { static: false }) chatform: ElementRef<HTMLInputElement> = {} as ElementRef;
  @ViewChild('messageField', { static: false }) messageField: ElementRef<HTMLInputElement> = {} as ElementRef;
  scrollToTop: number = 1;

  ngOnInit(): void {

    var url = "";
    if (environment.production == true) {
      console.log("Production true");
      url = this.authService.baseApiUrl + 'echo';
    }
    else {
      console.log("Production false");
      url = environment.apiUrl + 'echo';
    }

    // -----------------------------------------------------------------hubConnection for SignalR-------------------------------------------------------------------------------------
    //this.hubConnection = new HubConnectionBuilder().withUrl(url, {
    //  skipNegotiation: true,
    //  transport: HttpTransportType.WebSockets
    //}
    //).build();

    //this.hubConnection.start()
    //  .then(() => {
    //    console.log("Connection started")
    //  })
    //  .catch(err => {
    //    console.error(err);
    //  })

    // using ! operator to let compiler know that the object is not undefined
    this.authService.hubConnection!.on("SendToHomeController", (data) => {
      //this.messages.push(msg);
      this.createChatDataList(data);
    });
    // -----------------------------------------------------------------hubConnection for SignalR-------------------------------------------------------------------------------------


    this.getChatMessages();

    // Refreshing the lastSeen in database
    this.saveUserAndRoomDetail()
  }


  ngOnDestroy(): void {
    this.saveUserAndRoomDetail();
  }


  onFormSubmit(chatform: any) {
    if (chatform.message != '') {
      var urlToSendMessage = this.authService.baseApiUrl + this.authService.sendMessage_endpoint;
      const body = {
        roomId: this.selectedRoomId,
        userId: this.loggedInUserId,
        message: chatform.message
      };
      this.http.post<any>(urlToSendMessage, body).subscribe(data => {

        // Calling the echo method to invoke SignalR method
        if (this.authService.hubConnection != undefined) {
          this.authService.hubConnection.invoke("Echo", this.selectedRoomId);
        }
        else {
          console.log("Hub Connection undefined")
        }

      });


      // This is to clear the message in the form
      chatform.message = '';
      // This is to clear the message field in the form
      this.messageField.nativeElement.value = '';
    }
  }

  getChatMessages() {
    var urlToGetMessages = this.authService.baseApiUrl + this.authService.getMessages_endpoint;
    let parameters = new HttpParams().set("roomId", this.selectedRoomId).set("userId", this.loggedInUserId); //Create new HttpParams
    this.http.get<any>(urlToGetMessages, { params: parameters }).subscribe(data => {

      this.createChatDataList(data);

    });

  }

  createChatDataList(rawChatData: any) {
    var firstUnreadMessageFlag = false;   // first flag
    var isFirstUnreadMessageFound = false; // second flag to validate first flag
    var indexOfFirstUnreadMessage = 0;

    if (rawChatData) {
      if (rawChatData.roomId == this.selectedRoomId) {
        this.chatDataToDisplayList = [];
        var todayDate = new Date();
        var messageDateTimeToSet;

        var messageList = rawChatData.messages;
        for (let i = 0; i < messageList.length; i++) {
          var datetime = new Date(messageList[i].timestamp);
          var amOrPm = this.getAMorPM(datetime.getHours());

          if (rawChatData.loggedInUserLastSeen < messageList[i].timestamp) {

            this.numberOfUnseenMessages++;
            if (!isFirstUnreadMessageFound) {
              firstUnreadMessageFlag = true;
              isFirstUnreadMessageFound = true;
              indexOfFirstUnreadMessage = i;
            }
          }

          var messageTimeToSet = this.addZero(this.getTimeByAmPM(datetime.getHours())) + ":" + this.addZero(datetime.getMinutes()) + amOrPm;;
          if (datetime.getDate() == todayDate.getDate() && datetime.getMonth() == todayDate.getMonth() && datetime.getFullYear() == todayDate.getFullYear()) {
            messageDateTimeToSet = messageTimeToSet;
          }
          else {
            messageDateTimeToSet = this.month[datetime.getMonth()] + " " + datetime.getDate() + ", " + messageTimeToSet;
          }
          this.chatDataToDisplayList.push({
            email: messageList[i].userName,
            userName: messageList[i].userName,
            message: messageList[i].message,
            messageDateTime: messageDateTimeToSet,
            isFirstUnreadMessage: firstUnreadMessageFlag
          })

          //resetting the flag
          firstUnreadMessageFlag = false;
        }

        if (!isFirstUnreadMessageFound) {
          this.scrollToLastItem();
        }
        else {
          this.scrollToSpecificDiv(indexOfFirstUnreadMessage);
        }
      }
    }
  }

  addZero(i: any) {
    if (i < 10) { i = "0" + i }
    return i;
  }

  getAMorPM(i: any) {
    if (i >= 12) { return " pm" }
    else { return " am" }
  }

  getTimeByAmPM(hour: any) {
    if (hour > 12) {
      return hour - 12;
    }
    else {
      return hour;
    }
  }

  onLogOut() {
    this.authService.logout();
  }

  onBackButtonClicked() {
    this.router.navigate(['roomlist']);
  }

  scrollToLastItem() {

    setTimeout(() => this.scrollToTop = this.chatcontent.nativeElement.scrollHeight, 100);

  }

  scrollToSpecificDiv(index:number) {
    setTimeout(() => {

      if (document.getElementById('wave' + (index - 1)) != null) {
        //using ! operator to make compiler know that it is not null
        document.getElementById('wave' + (index - 1))!.scrollIntoView()
      }
      else if (document.getElementById('wave'+index) != null) {

        //using ! operator to make compiler know that it is not null
        document.getElementById('wave' + index)!.scrollIntoView()
      }
    },
      500);

    console.log('scrolled to ' + index);
  }

  saveUserAndRoomDetail() {
    var urlToGetRooms = this.authService.baseApiUrl + this.authService.updatelastseendt_endpoint;

    const body = {
      roomId: this.selectedRoomId,
      userId: this.loggedInUserId,
    };

    this.http.post<any>(urlToGetRooms, body).subscribe(res => {
      console.log(res);
    })

  }
}
