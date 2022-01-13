import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-feed-view',
  templateUrl: './feed-view.component.html',
  styleUrls: ['./feed-view.component.css']
})
export class FeedViewComponent implements OnInit {
  model: any = {};
  items: number[] = [1,2,3,4,5];

  constructor() { }

  ngOnInit(): void {
  }

  Publish(){
    console.log("Published"+this.model.status);
  }

}
