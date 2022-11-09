import { Injectable, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar'
import { HttpClient, HttpParams, JsonpClientBackend } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})


export class ChatapplicationServiceService {

  readonly logIn_endpoint = "user/login";
  readonly createUser_endpoint = "user/createuser";
  readonly getRooms_endpoint = "room/getrooms";
  readonly createRoom_endpoint = "room/createroom";
  readonly sendMessage_endpoint = "chat/sendmessage";
  readonly getMessages_endpoint = "chat/getmessages";
  chatRoomToOpen = null;
  baseApiUrl = "";

  isUserSignedIn() {
    if (localStorage.getItem('currentUser')) {
      return true;
    }
    else return false;
  }

  constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') baseUrl: string, public _snackBar: MatSnackBar) {
    this.baseApiUrl = baseUrl;

  }

  signup(email: string, password: string) {
    var urlToLogin = this.baseApiUrl + this.createUser_endpoint;
    let parameters = new HttpParams().set("username", "test")
      .set("email", email)
      .set("password", password);


    var result = this.http.post(urlToLogin, { params: parameters });
    var i = 7;
  }

  login(email: string, password: string) {
    var urlToLogin = this.baseApiUrl + this.logIn_endpoint;
    let parameters = new HttpParams().set("email", email).set("password", password); //Create new HttpParams

    this.http.get<any>(urlToLogin, { params: parameters }).subscribe(data => {
      localStorage.setItem('CurrentUser', JSON.stringify(data))

      this.router.navigate(['roomlist']);
      this._snackBar.open("Signed in as " + email, "Ok")
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
    //if (email) {
    //  sendPasswordResetEmail(getAuth(), email)
    //    .then((response) => {
    //      console.log('')
    //      this._snackBar.open("Password reset email has been sent, Please check your inbox including spam folder.", "Ok");
    //    })
    //    .catch((error) => {
    //      const errorCode = error.code;
    //      const errorMessage = error.message;
    //      this._snackBar.open("There is some error sending the password reset email. Please enter the correct email.", "Ok");
    //    });
    //}
  }
}
