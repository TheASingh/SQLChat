import { Injectable, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar'
import { HttpClient, HttpParams, JsonpClientBackend } from '@angular/common/http';
import { environment } from '../environments/environment';
import { FailedToNegotiateWithServerError } from '@microsoft/signalr/dist/esm/Errors';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';


@Injectable({
  providedIn: 'root'
})


export class ChatapplicationServiceService {

  readonly logIn_endpoint = "user/login";
  readonly createUser_endpoint = "user/createuser";
  readonly verifyandapprovetoken_endpoint = "user/verifyandapprovetoken";
  readonly sendresetpasswordemail_endpoint = "user/sendresetpasswordemail";
  readonly resetpassword_endpoint = "user/resetpassword";

  readonly getRooms_endpoint = "room/getrooms";
  readonly createRoom_endpoint = "room/createroom";

  readonly sendMessage_endpoint = "chat/sendmessage";
  readonly getMessages_endpoint = "chat/getmessages";

  readonly updatelastseendt_endpoint = "userroom/updatelastseendt";

  hubConnection: HubConnection | undefined;

  chatRoomToOpen = null;
  baseApiUrl = "";
  baseUrlOfProject = "";
  signalRUrl = "";


  // There are two ways:

  // 1. Either by declaring a BehaviorSubject or Subject and make it Observable. BehaviorSubject can be initialized with a value, but Subject can't be.
  // definition: BehaviorSubject or Subject acts as Observer as well as Observable.
  //  a. Observer means, we can set a value to it using next() method.
  //  b. Observable means, we can subscribe to it
  //showLogInLoadingBehaviorSubject = new BehaviorSubject<boolean>(false);

  // making it an observable
  //showLogInLoadingObservable = this.showLogInLoadingBehaviorSubject.asObservable();


  // 2. Diretly use BehaviorSubject or Subject
  showLogInLoadingSubject = new Subject<boolean>();


  showSignUpLoadingBehaviorSubject = new BehaviorSubject<boolean>(false);


  isUserSignedIn() {
    if (localStorage.getItem('currentUser')) {
      return true;
    }
    else return false;
  }

  constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') baseUrl: string, public _snackBar: MatSnackBar) {
    this.baseApiUrl = baseUrl;
    this.baseUrlOfProject = baseUrl;

    if (environment.production == false) {
      console.log("Production false");
      this.baseUrlOfProject = environment.projectUrlFrontEnd;
      this.signalRUrl = environment.apiUrl + 'echo';
    }
    else {
      console.log("Production true");
      this.signalRUrl = this.baseApiUrl + 'echo';
    }


    // -----------------------------------------------------------------hubConnection for SignalR-------------------------------------------------------------------------------------
    this.hubConnection = new HubConnectionBuilder().withUrl(this.signalRUrl, {
      skipNegotiation: true,
      transport: HttpTransportType.WebSockets
    }
    ).build();

    this.hubConnection.start()
      .then(() => {
        console.log("Connection started")
      })
      .catch(err => {
        console.error(err);
      })

    // -----------------------------------------------------------------hubConnection for SignalR-------------------------------------------------------------------------------------


  }

  signup(username: string, email: string, password: string) {
    this.showSignUpLoadingBehaviorSubject.next(true);

    var urlToCreateUser = this.baseApiUrl + this.createUser_endpoint;
    const body = {
      username: username,
      email: email,
      password: password,
      projectBaseUrl: this.baseUrlOfProject
    };

    this.http.post<any>(urlToCreateUser, body).subscribe(result => {

      // Calling the echo method to invoke SignalR method
      if (result.success) {
        this._snackBar.open("Please verify your email, a verification email has been sent to your email address: " + email, "Ok")
      }
      else {
        this._snackBar.open(result.error, "Ok")
      }

      this.showSignUpLoadingBehaviorSubject.next(false);

    });
  }

  login(email: string, password: string) {
    //this.showLogInLoadingBehaviorSubject.next(true);
    this.showLogInLoadingSubject.next(true);

    var urlToLogin = this.baseApiUrl + this.logIn_endpoint;
    let parameters = new HttpParams().set("email", email).set("password", password); //Create new HttpParams
    var responseToReturn = false;
    this.http.get<any>(urlToLogin, { params: parameters }).subscribe(result => {

      if (result.success) {
        localStorage.setItem('CurrentUser', JSON.stringify(result.data))

        this.router.navigate(['roomlist']);
        this._snackBar.open("Signed in as " + email, "Ok")
      }
      else {
        if (result.error) {
          this._snackBar.open(result.error, "Ok")
        }
        else {
          this._snackBar.open("Invalid email or password", "Ok")
        }
      }
      // this.showLogInLoadingBehaviorSubject.next(false);
      this.showLogInLoadingSubject.next(false);
    })




  }

  logout() {
    localStorage.removeItem('CurrentUser');
    this.router.navigate(['/home']);
  }

  forgotPassword(email: string) {
    if (email) {

      var urlToSendForgotPasswordEmail = this.baseApiUrl + this.sendresetpasswordemail_endpoint;
      let parameters = new HttpParams().set("email", email).set("projectBaseUrl", this.baseUrlOfProject);

      this.http.get<any>(urlToSendForgotPasswordEmail, { params: parameters }).subscribe(result => {
        if (result.success) {
          this._snackBar.open("Reset password email has been sent to your account: " + email, "Ok");
        }
        else {
          this._snackBar.open(result.error);
        }
      })

    }
  }

  resetPassword(token: string, password: string) {
    var urlToCreateUser = this.baseApiUrl + this.resetpassword_endpoint;

    const body = {
      token: token,
      password: password
    };

    this.http.post<any>(urlToCreateUser, body).subscribe(result => {

      // Calling the echo method to invoke SignalR method
      if (result.success) {
        this._snackBar.open("Password has been reset successfully, please proceed with login", "Ok");
      }
      else {
        this._snackBar.open(result.error, "Ok")
      }

    });
  }
}
