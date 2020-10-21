import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-auth-header',
  templateUrl: './auth-header.component.html',
  styleUrls: ['./auth-header.component.css'],
})
export class AuthHeaderComponent implements OnInit {
  @Input() title: string;
  constructor() {}

  ngOnInit(): void {}
}
