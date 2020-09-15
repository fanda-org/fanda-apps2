import { Component, OnInit } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css'],
})
export class WelcomeComponent implements OnInit {
  constructor(private nzMessageService: NzMessageService) {}

  ngOnInit(): void {
    console.log('welcome:onInit');
  }

  onSaveClick(): void {
    this.nzMessageService.success('Save button clicked!');
  }
}
