import { Component, OnInit } from '@angular/core';
import { CodeService } from '../api/services';

@Component({
  selector: 'app-code-view',
  templateUrl: './code-view.component.html',
  styleUrls: ['./code-view.component.css']
})
export class CodeViewComponent implements OnInit {

  constructor(private svc : CodeService) { }

  code : string = '';
  loaded : boolean = false;
  data : string = '';
  saving : boolean = false;
  deleting : boolean = false;
  loading : boolean = false;
  isError = false;

  ngOnInit(): void {
  }

  load() { 
    this.loading = true;
    this.svc.postApiCode({code: this.code}).subscribe(x => {
      this.data = x;
      this.loaded = true;
      this.loading = false;
    },e => {
      this.isError = true;
      this.loading = false;
    });
  }

  save() {
    this.saving = true;
    this.svc.putApiCode({code : this.code, data: this.data }).subscribe(x => {
      this.saving = false;
    });
  }

  delete()  {
    this.deleting = true;
    this.svc.postApiCodeDelete({code:this.code}).subscribe(x => {
      this.deleting = false;
    });
  }

}
