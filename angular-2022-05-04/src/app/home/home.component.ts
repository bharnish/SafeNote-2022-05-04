import { Component, OnInit } from '@angular/core';
import { CryptoService } from '../api/services';
import { PostData } from '../api/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private cryptoSvc : CryptoService) { }

  pd : PostData = { data: '' };
  url : string = '';
  loading : boolean = false;

  ngOnInit(): void {
  }

  submit() : void  {
    this.loading = true;
    this.cryptoSvc.postApiCryptoCreate(this.pd).subscribe(x => {
      this.url = x;
      this.loading = false;
    });
  }

  reset() { 
    this.url = '';
  }
}
