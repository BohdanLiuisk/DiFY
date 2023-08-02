import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpRequestConfig } from '@core/models/http-request.config';
import { QueryResponse } from '@core/models/query-response';
import { Observable, of, switchMap, throwError } from 'rxjs';

export class HttpInternalService {
  constructor(protected http: HttpClient, protected baseUrl: string) { }

  public getRequest<T>(url: string, config?: HttpRequestConfig): Observable<T> {
    this.setQueryResponseTrue(config);
    const request = this.http.get<QueryResponse<T> | T>(this.buildUrl(url), { 
      headers: config?.headers, params: config?.httpParams 
    });
    return this.handleResponse(request, config);
  }

  public postRequest<T>(url: string, payload: object, config?: HttpRequestConfig): Observable<T> {
    this.setQueryResponseTrue(config);
    const request = this.http.post<QueryResponse<T> | T>(this.buildUrl(url), payload, { 
      headers: config?.headers, params: config?.httpParams 
    });
    return this.handleResponse(request, config);
  }

  public putRequest<T>(url: string, payload?: object, headers?: HttpHeaders, httpParams?: any): Observable<T> {
    return this.http.put<T>(this.buildUrl(url), payload, { headers: headers, params: httpParams });
  }

  public patchRequest<T>(url: string, payload?: object, headers?: HttpHeaders,) {
    return this.http.patch<T>(this.buildUrl(url), payload, { headers: headers});
  }

  public deleteRequest<T>(url: string, headers?: HttpHeaders, httpParams?: any): Observable<T> {
    return this.http.delete<T>(this.buildUrl(url), { headers: headers, params: httpParams });
  }

  private setQueryResponseTrue(config?: HttpRequestConfig) {
    if (config) {
      config.isQueryResponse ??= true;
    }
  }

  private handleResponse<T>(request: Observable<QueryResponse<T> | T>, config?: HttpRequestConfig): Observable<T> {
    if(config && !config.isQueryResponse) {
      return request as Observable<T>;
    }
    return request.pipe(
      switchMap((response: any) => {
        const resp = response as QueryResponse<T>;
        if (resp.success) {
          return of(resp.body);
        } else {
          return throwError(() => new Error(resp.error)); 
        }
      })
    );
  }

  protected buildUrl(url: string): string {
    return this.baseUrl + url;
  }
}
