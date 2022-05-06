import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CryptoService } from '../api/services';

@Component({
  selector: 'app-note-view',
  templateUrl: './note-view.component.html',
  styleUrls: ['./note-view.component.css']
})
export class NoteViewComponent implements OnInit {

  constructor(private route: ActivatedRoute, private cryptSvc: CryptoService) { }

  data: string = '';
  loading: boolean = false;
  notfound : boolean = false;

  ngOnInit(): void {
  }

  fetch(): void {
    let i = '';
    this.route.queryParamMap.subscribe(x => {
      i = x.get('i') ?? '';
    });

    let k = '';
    this.route.paramMap.subscribe(x => {
      k = x.get('key') ?? '';
    });

    this.loading = true;
    this.cryptSvc.postApiCryptoRead({ key: k, iv: i }).subscribe(x => {
      this.data = x;
      this.loading = false;
    }, err => {
      this.loading = false;
      this.notfound = true;
    });

  }

}
