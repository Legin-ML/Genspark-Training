import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../environments/environment'
import { LoginModel } from '../../models/LoginModel';
import { Observable, tap } from 'rxjs';
import {jwtDecode} from 'jwt-decode'
import { AuthService } from '../auth/auth.service';
import { SignUpModel } from '../../models/SignUpModel';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private url = environment.apiUrlBase;

  constructor(private http: HttpClient, private auth: AuthService) {}

  requestLogin(loginData: LoginModel): Observable<any> {
    return this.auth.requestLogin(loginData)
  }

  requestSignUp(signupData: SignUpModel) :  Observable<any> {
    return this.auth.requestSignIn(signupData)
  }
  
}
