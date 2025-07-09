import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { Observable, tap } from 'rxjs';
import { LoginModel } from '../../models/LoginModel';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { SignUpModel } from '../../models/SignUpModel';
import { UserModel } from '../../models/UserModel';

export interface DecodedToken {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  exp: number;
  iss: string;
  aud: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private url = environment.apiUrlBase;

  constructor(private http: HttpClient) { }

  requestLogin(loginData: LoginModel): Observable<any> {
    return this.http.post<{ token: string }>(`${this.url}/auth/login`, loginData).pipe(
      tap(res => {
        localStorage.setItem('auth_token', res.token)
      })
    );
  }

  requestSignIn(signupData :  SignUpModel) : Observable<any> {
    return this.http.post<{user : UserModel}>(`${this.url}/users`, signupData).pipe(
      tap(res => {
        
      })
    )
  }

  logout() {
    localStorage.removeItem('auth_token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('auth_token');
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  getUserRole(): string | null {
    const token = this.getToken();

    if (!token) return null;

    try {
      const decoded: DecodedToken = jwtDecode(token);
      return decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    } catch (err) {
      console.error('Token decoding failed', err);
      return null;
    }
  }

  getUserId(): string | null {
    const token = this.getToken();

    if (!token) return null;

    try {
      const decoded: DecodedToken = jwtDecode(token);
      return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    } catch (err) {
      console.error('Token decoding failed', err);
      return null;
    }
  }
  

  isAdmin() : boolean {
    if (this.getUserRole()){
      if (this.getUserRole()?.toLowerCase() == "admin"){
        return true
      }
    }
    return false
  }
}
