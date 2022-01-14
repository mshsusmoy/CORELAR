import { Component, OnInit, ViewChild } from '@angular/core';
import { Status } from '../_models/status';
import { StatusService } from '../_services/status.service';
import { NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-feed-view',
  templateUrl: './feed-view.component.html',
  styleUrls: ['./feed-view.component.css']
})
export class FeedViewComponent implements OnInit {
  @ViewChild('statusForm') statusForm: NgForm;
  model: any = {};
  statuses : Status[];
  items: number[] = [1,2,3,4,5];
  user: User;

  constructor(public statusService: StatusService, private accountService: AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    console.log(this.user.token);
    this.loadStatuses();
  }

  loadStatuses(){
    this.statusService.getStatuses().subscribe(respose => {
      this.statuses = respose;
    });
  }

  Publish(){
    this.statusService.postStatus(this.model.status).then(() => {
      this.statusForm.reset();
      this.loadStatuses();
    });
  }

}

