import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatapplicationServiceService } from '../chatapplicationservice.service';

@Component({
  selector: 'app-verifyemail',
  templateUrl: './verifyemail.component.html',
  styleUrls: ['./verifyemail.component.css']
})
export class VerifyemailComponent implements OnInit {

  token: string = '';
  constructor(private router: Router,
    private http: HttpClient,
    private activatedRoute: ActivatedRoute,
    private authService: ChatapplicationServiceService,
    public _snackBar: MatSnackBar  ) {


    this.token = this.activatedRoute.snapshot.params.token;

  }

  ngOnInit(): void {

    var urlToVerifyToken = this.authService.baseApiUrl + this.authService.verifyandapprovetoken_endpoint;
    let parameters = new HttpParams().set("token", this.token);

    this.http.get<any>(urlToVerifyToken, { params: parameters }).subscribe(result => {

      if (result.success) {
        this._snackBar.open("Your account is now verified, you can login now.", "Ok")
      }
      else {
        this._snackBar.open(result.error, "Ok")
      }
    })
  }

}
