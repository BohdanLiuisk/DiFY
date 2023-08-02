import { HttpHeaders } from "@angular/common/http";

export interface HttpRequestConfig {
    headers?: HttpHeaders;
    httpParams?: any;
    isQueryResponse: boolean;
}
