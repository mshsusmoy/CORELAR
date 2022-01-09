import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-about-dev',
  templateUrl: './about-dev.component.html',
  styleUrls: ['./about-dev.component.css']
})
export class AboutDevComponent implements OnInit {
  @Output() cancelAboutDev = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  cancel(){
    this.cancelAboutDev.emit(false);
  }
  

}
