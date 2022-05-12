/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders } from '@angular/common/http';
import { BaseService as __BaseService } from '../base-service';
import { ApiConfiguration as __Configuration } from '../api-configuration';
import { StrictHttpResponse as __StrictHttpResponse } from '../strict-http-response';
import { Observable as __Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

import { ShortenerReadData } from '../models/shortener-read-data';
import { ShortenerPostData } from '../models/shortener-post-data';
@Injectable({
  providedIn: 'root',
})
class ShortenerService extends __BaseService {
  static readonly postApiShortenerReadPath = '/api/Shortener/read';
  static readonly postApiShortenerCreatePath = '/api/Shortener/create';

  constructor(
    config: __Configuration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param body undefined
   * @return Success
   */
  postApiShortenerReadResponse(body?: ShortenerReadData): __Observable<__StrictHttpResponse<string>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = body;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/Shortener/read`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'text'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<string>;
      })
    );
  }
  /**
   * @param body undefined
   * @return Success
   */
  postApiShortenerRead(body?: ShortenerReadData): __Observable<string> {
    return this.postApiShortenerReadResponse(body).pipe(
      __map(_r => _r.body as string)
    );
  }

  /**
   * @param body undefined
   * @return Success
   */
  postApiShortenerCreateResponse(body?: ShortenerPostData): __Observable<__StrictHttpResponse<string>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = body;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/Shortener/create`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'text'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<string>;
      })
    );
  }
  /**
   * @param body undefined
   * @return Success
   */
  postApiShortenerCreate(body?: ShortenerPostData): __Observable<string> {
    return this.postApiShortenerCreateResponse(body).pipe(
      __map(_r => _r.body as string)
    );
  }
}

module ShortenerService {
}

export { ShortenerService }
