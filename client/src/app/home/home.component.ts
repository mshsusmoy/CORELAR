import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  learnMore = false;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle(){
    this.learnMore ? this.learnMore = !this.learnMore : false;
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

  learnMoreToggle(){
    this.registerMode ? this.registerMode = !this.registerMode : false;
    this.learnMore = !this.learnMore;
  }

  cancelLearnMoreMode(event: boolean){
    this.learnMore = event;
  }
}
