import { Component, OnInit } from '@angular/core';
import { CryptoService, ShortenerService } from '../api/services';
import { PostData } from '../api/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private cryptoSvc : CryptoService, private shortSvc : ShortenerService) { }

  data : string = '';
  url : string = '';
  loading : boolean = false;
  loadingShort : boolean = false;

  ngOnInit(): void {
  }

  submit() : void  {
    this.loading = true;
    this.cryptoSvc.postApiCryptoCreate({ data : this.data }).subscribe(x => {
      this.url = x;
      this.loading = false;
    });
  }

  submitShort() : void  {
    this.loadingShort = true;
    this.shortSvc.postApiShortenerCreate({ data : this.data }).subscribe(x => {
      this.url = 's/' + x;
      this.loadingShort = false;
    });
  }

  reset() { 
    this.url = '';
  }
}
