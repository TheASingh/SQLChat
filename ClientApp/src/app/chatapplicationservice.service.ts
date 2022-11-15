import { Injectable, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar'
import { HttpClient, HttpParams, JsonpClientBackend } from '@angular/common/http';
import { environment } from '../environments/environment';
import { FailedToNegotiateWithServerError } from '@microsoft/signalr/dist/esm/Errors';


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

  chatRoomToOpen = null;
  baseApiUrl = "";
  baseUrlOfProject = "";

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
      this.baseUrlOfProject = environment.projectUrlFrontEnd;
    }

  }

  signup(username: string, email: string, password: string) {
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

    });
  }

  login(email: string, password: string) {
    var urlToLogin = this.baseApiUrl + this.logIn_endpoint;
    let parameters = new HttpParams().set("email", email).set("password", password); //Create new HttpParams

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
    })


  }

  verifyEmail(user: any) {
    //sendEmailVerification(user)
    //  .then((result) => {
    //    this._snackBar.open("Please check your inbox to verify the email address", "Ok")
    //  });
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
