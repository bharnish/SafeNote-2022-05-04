import { Component, OnInit } from '@angular/core';
import { LockerService } from '../api/services';

@Component({
  selector: 'app-locker',
  templateUrl: './locker.component.html',
  styleUrls: ['./locker.component.css']
})
export class LockerComponent implements OnInit {

  constructor(private svc:LockerService) { }

  url = '';
  isLoading = false;

  ngOnInit(): void {
  }

  submit() {
    this.isLoading = true;
    this.svc.getApiLockerCreate().subscribe(x => {
      this.url = x;
      this.isLoading = false;
    });
  }
}
