import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatapplicationServiceService } from '../chatapplicationservice.service';

@Component({
  selector: 'app-forgotpassworduserentry',
  templateUrl: './forgotpassworduserentry.component.html',
  styleUrls: ['./forgotpassworduserentry.component.css']
})
export class ForgotpassworduserentryComponent implements OnInit {

  token: string = '';

  constructor(private authService: ChatapplicationServiceService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {

    this.token = this.activatedRoute.snapshot.params.token;

  }

  ngOnInit(): void {
  }

  onResetPasswordClick(value: any): void {
    this.authService.resetPassword(this.token, value.newPassword)


  }

  onProceedWithLoginClick(): void {
    this.router.navigate(['home']);

  }

}
