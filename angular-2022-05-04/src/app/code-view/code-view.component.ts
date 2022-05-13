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
  saved : boolean = false;
  deleted : boolean = false;
  loading : boolean = false;

  ngOnInit(): void {
  }

  load() { 
    this.loading = true;
    this.svc.getApiCodeCode(this.code).subscribe(x => {
      this.data = x;
      this.loaded = true;
      this.loading = false;
    });
  }

  save() {
    this.saved = false;
    this.deleted = false;
    this.svc.putApiCodeCode({ code : this.code, body : { data: this.data }}).subscribe(x => {
      this.saved = true;
    });
  }

  delete()  {
    this.saved = false;
    this.deleted = false;
    this.svc.deleteApiCodeCode(this.code).subscribe(x => {
      this.deleted = true;
    });
  }

}
