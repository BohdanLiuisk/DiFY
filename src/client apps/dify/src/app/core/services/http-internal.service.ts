import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export class HttpInternalService {
  constructor(protected http: HttpClient, protected baseUrl: string) { }

  public getRequest<T>(url: string, headers?: HttpHeaders, httpParams?: any): Observable<T> {
    return this.http.get<T>(this.buildUrl(url), { headers: headers, params: httpParams });
  }

  public postRequest<T>(url: string, payload: object, headers?: HttpHeaders, httpParams?: any): Observable<T> {
    return this.http.post<T>(this.buildUrl(url), payload, { headers: headers, params: httpParams });
  }

  public putRequest<T>(url: string, payload: object, headers?: HttpHeaders, httpParams?: any): Observable<T> {
    return this.http.put<T>(this.buildUrl(url), payload, { headers: headers, params: httpParams });
  }

  public patchRequest<T>(url: string, payload: object, headers?: HttpHeaders,) {
    return this.http.patch<T>(this.buildUrl(url), payload, { headers: headers});
  }

  public deleteRequest<T>(url: string, headers?: HttpHeaders, httpParams?: any): Observable<T> {
    return this.http.delete<T>(this.buildUrl(url), { headers: headers, params: httpParams });
  }

  protected buildUrl(url: string): string {
    return this.baseUrl + url;
  }
}
