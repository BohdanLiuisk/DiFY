export interface QueryResponse<T> {
  success: boolean;
  body: T;
  error: string;
}
