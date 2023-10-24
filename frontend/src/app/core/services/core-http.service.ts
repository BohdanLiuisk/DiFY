import { HttpInternalService } from './http-internal.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';

@Injectable()
export class CoreHttpService extends HttpInternalService {
  constructor(http: HttpClient) {
    super(http, environment.coreUrl);
  }
}
