/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders } from '@angular/common/http';
import { BaseService as __BaseService } from '../base-service';
import { ApiConfiguration as __Configuration } from '../api-configuration';
import { StrictHttpResponse as __StrictHttpResponse } from '../strict-http-response';
import { Observable as __Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

import { CodeData } from '../models/code-data';
@Injectable({
  providedIn: 'root',
})
class CodeService extends __BaseService {
  static readonly getApiCodeCodePath = '/api/Code/{code}';
  static readonly putApiCodeCodePath = '/api/Code/{code}';
  static readonly deleteApiCodeCodePath = '/api/Code/{code}';

  constructor(
    config: __Configuration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param code undefined
   * @return Success
   */
  getApiCodeCodeResponse(code: string): __Observable<__StrictHttpResponse<string>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/Code/${encodeURIComponent(String(code))}`,
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
   * @param code undefined
   * @return Success
   */
  getApiCodeCode(code: string): __Observable<string> {
    return this.getApiCodeCodeResponse(code).pipe(
      __map(_r => _r.body as string)
    );
  }

  /**
   * @param params The `CodeService.PutApiCodeCodeParams` containing the following parameters:
   *
   * - `code`:
   *
   * - `body`:
   */
  putApiCodeCodeResponse(params: CodeService.PutApiCodeCodeParams): __Observable<__StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    __body = params.body;
    let req = new HttpRequest<any>(
      'PUT',
      this.rootUrl + `/api/Code/${encodeURIComponent(String(params.code))}`,
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
   * @param params The `CodeService.PutApiCodeCodeParams` containing the following parameters:
   *
   * - `code`:
   *
   * - `body`:
   */
  putApiCodeCode(params: CodeService.PutApiCodeCodeParams): __Observable<null> {
    return this.putApiCodeCodeResponse(params).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param code undefined
   */
  deleteApiCodeCodeResponse(code: string): __Observable<__StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'DELETE',
      this.rootUrl + `/api/Code/${encodeURIComponent(String(code))}`,
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
   * @param code undefined
   */
  deleteApiCodeCode(code: string): __Observable<null> {
    return this.deleteApiCodeCodeResponse(code).pipe(
      __map(_r => _r.body as null)
    );
  }
}

module CodeService {

  /**
   * Parameters for putApiCodeCode
   */
  export interface PutApiCodeCodeParams {
    code: string;
    body?: CodeData;
  }
}

export { CodeService }
