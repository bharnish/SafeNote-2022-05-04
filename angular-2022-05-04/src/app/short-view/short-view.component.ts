import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShortenerService } from '../api/services';

@Component({
  selector: 'app-short-view',
  templateUrl: './short-view.component.html',
  styleUrls: ['./short-view.component.css']
})
export class ShortViewComponent implements OnInit {

  constructor(private route: ActivatedRoute, private cryptSvc: ShortenerService) { }

  data: string = '';
  loading: boolean = false;
  notfound : boolean = false;

  ngOnInit(): void {
  }

  fetch(): void {
    let k = '';
    this.route.paramMap.subscribe(x => {
      k = x.get('key') ?? '';
    });

    this.loading = true;
    this.cryptSvc.postApiShortenerRead({ id : k }).subscribe(x => {
      this.data = x;
      this.loading = false;
    }, err => {
      this.loading = false;
      this.notfound = true;
    });

  }

}
