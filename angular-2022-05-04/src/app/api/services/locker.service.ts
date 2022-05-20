/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders } from '@angular/common/http';
import { BaseService as __BaseService } from '../base-service';
import { ApiConfiguration as __Configuration } from '../api-configuration';
import { StrictHttpResponse as __StrictHttpResponse } from '../strict-http-response';
import { Observable as __Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

import { LockerCode } from '../models/locker-code';
import { LockerCodeData } from '../models/locker-code-data';
@Injectable({
  providedIn: 'root',
})
class LockerService extends __BaseService {
  static readonly getApiLockerCreatePath = '/api/Locker/create';
  static readonly postApiLockerReadPath = '/api/Locker/read';
  static readonly putApiLockerUpdatePath = '/api/Locker/update';
  static readonly postApiLockerDeletePath = '/api/Locker/delete';

  constructor(
    config: __Configuration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @return Success
   */
  getApiLockerCreateResponse(): __Observable<__StrictHttpResponse<string>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/Locker/create`,
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
   * @return Success
   */
  getApiLockerCreate(): __Observable<string> {
    return this.getApiLockerCreateResponse().pipe(
      __map(_r => _r.body as string)
    );
  }

  /**
   * @param body undefined
   * @return Success
   */
  postApiLockerReadResponse(body?: LockerCode): __Observable<__StrictHttpResponse<string>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = body;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/Locker/read`,
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
  postApiLockerRead(body?: LockerCode): __Observable<string> {
    return this.postApiLockerReadResponse(body).pipe(
      __map(_r => _r.body as string)
    );
  }

  /**
   * @param body undefined
   */
  putApiLockerUpdateResponse(body?: LockerCodeData): __Observable<__StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = body;
    let req = new HttpRequest<any>(
      'PUT',
      this.rootUrl + `/api/Locker/update`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param body undefined
   */
  putApiLockerUpdate(body?: LockerCodeData): __Observable<null> {
    return this.putApiLockerUpdateResponse(body).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param body undefined
   */
  postApiLockerDeleteResponse(body?: LockerCode): __Observable<__StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = body;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/Locker/delete`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param body undefined
   */
  postApiLockerDelete(body?: LockerCode): __Observable<null> {
    return this.postApiLockerDeleteResponse(body).pipe(
      __map(_r => _r.body as null)
    );
  }
}

module LockerService {
}

export { LockerService }
