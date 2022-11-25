import { Component, OnInit, ElementRef, ViewChild, AfterViewChecked, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ForgotpasswordComponent } from '../forgotpassword/forgotpassword.component';
import { ChatapplicationServiceService } from '../chatapplicationservice.service';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { environment } from '../../environments/environment';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})


export class HomeComponent implements OnInit {

  routerForNavigatingToOtherComponents: any;
  emailToResetPasswordFor: any;
  showLogInWindow: boolean = true;
  showLogInProgressBar: boolean = false;
  showSignUpProgressBar: boolean = false;
  public message: string = "";
  public messages: string[] = [];

  public hubConnection: HubConnection | undefined;



  constructor(private authService: ChatapplicationServiceService,
    private _router: Router,
    public dialog: MatDialog
  ) {
    this.routerForNavigatingToOtherComponents = this._router;

    // There are two ways:

    // 1. Either by declaring a BehaviorSubject or Subject and make it Observable (check commented code in service file)
    // definition: BehaviorSubject or Subject acts as Observer as well as Observable.
    //  a. Observer means, we can set a value to it using next() method.
    //  b. Observable means, we can subscribe to it
    //this.authService.showLogInLoadingObservable.subscribe(res => {
    //  this.showLogInProgressBar = res;
    //});

    // 2. Diretly use BehaviorSubject or Subject

    this.authService.showLogInLoadingSubject.subscribe(res => {
      this.showLogInProgressBar = res;
    });

    this.authService.showSignUpLoadingBehaviorSubject.subscribe(res => {
      this.showSignUpProgressBar = res;
    })
    

  }

  ngOnInit(): void {
  }

  onLogInClick(value: any) {
    
    this.authService.login(value.email, value.password);
  }

  onRegisterClick(value: any) {
    this.authService.signup(value.usernameForRegistration, value.emailForRegistration, value.passwordForRegistration);
  }

  openForgotPasswordDialog() {
    let dialogRef = this.dialog.open(ForgotpasswordComponent,
      {
        width: '250px',
        data: { heading: 'Enter email for which you want to reset the password' }
      });

    dialogRef.afterClosed().subscribe(result => {
      this.emailToResetPasswordFor = result;
      this.authService.forgotPassword(this.emailToResetPasswordFor);
    })
  }

  onRegisterAsANewUserClick() {
    this.showLogInWindow = false;
  }

  onAlreadyHaveAnAccountClick() {
    this.showLogInWindow = true;
  }


}

