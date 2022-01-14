import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { PresenceService } from './presence.service';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { StatusService } from './_services/status.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'CORELAR APP';
  sub_title = 'Developed By Susmoy';
  users: any;

  constructor(private accountService: AccountService, private presence: PresenceService, private statusService: StatusService) {

  }
  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user'));
    if(user){
      this.accountService.setCurrentUser(user);
      this.presence.createHubConnection(user);
      this.statusService.createHubConnection(user);
    }
    
  }

}
