import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LockerService } from '../api/services';

@Component({
  selector: 'app-locker-view',
  templateUrl: './locker-view.component.html',
  styleUrls: ['./locker-view.component.css']
})
export class LockerViewComponent implements OnInit {

  constructor(private svc:LockerService, private routed:ActivatedRoute) { }

  data = '';
  code = '';
  saving = false;
  deleting = false;
  loaded = false;

  ngOnInit(): void {
    this.routed.paramMap.subscribe(x => {
      this.code = x.get('key')??'';

      this.svc.postApiLockerRead({code: this.code}).subscribe(x => {
        this.data = x;
        this.loaded = true;
      });
    });
  }

  save() {
    this.saving = true;
    this.svc.putApiLockerUpdate({code: this.code, data: this.data}).subscribe(x => {
      this.saving = false;
    })
   }
  delete() {
    this.deleting = true;
    this.svc.postApiLockerDelete({code: this.code}).subscribe(x => {
      this.deleting = false;
    })
   }
}
