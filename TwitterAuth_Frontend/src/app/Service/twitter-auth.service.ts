import { Injectable } from '@angular/core';
import { RequestToken } from '../Model/RequestToken.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TwitterAuthService {

  baseUrl = "https://localhost:44365/api/";

  constructor(private http: HttpClient) { }

getRequestToken(): Observable<RequestToken> {
  return this.http.get<RequestToken>(this.baseUrl +'TwitterClient/GetRequestToken');
}

getAccessToken(oauth_token: string, oauth_verifier: string): Observable<any> {
   const params = new HttpParams()
     .set('oauth_token', oauth_token)
     .set('oauth_verifier', oauth_verifier);
 
   return this.http.get(this.baseUrl + 'TwitterClient/sign-in-with-twitter', { params });
 }}
