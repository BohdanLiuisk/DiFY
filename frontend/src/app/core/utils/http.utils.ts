import { HttpRequest } from '@angular/common/http';

export function setBearerToHeader(request: HttpRequest<any>, token: string): HttpRequest<any> {
  return request.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
}
