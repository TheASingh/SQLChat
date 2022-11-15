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
  styleUrls: ['./home.component.css']
})


export class HomeComponent implements OnInit {

  routerForNavigatingToOtherComponents: any;
  emailToResetPasswordFor: any;
  showLogInWindow: boolean = true;
  public message: string = "";
  public messages: string[] = [];

  public hubConnection: HubConnection | undefined;



  constructor(private authService: ChatapplicationServiceService,
    private _router: Router,
    public dialog: MatDialog
  ) {
    this.routerForNavigatingToOtherComponents = this._router;
  }

  ngOnInit(): void {
  }

  onLogInClick(value: any) {
    this.authService.login(value.email, value.password);
  }

  onRegisterClick(value: any) {
    this.authService.signup(value.usernameForRegistration ,value.emailForRegistration, value.passwordForRegistration);
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

