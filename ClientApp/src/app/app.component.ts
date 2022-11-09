import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChatapplicationServiceService } from './chatapplicationservice.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(private authService: ChatapplicationServiceService, private router: Router) {

  }

  ngOnInit(): void {

  }
}
