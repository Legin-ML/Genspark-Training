import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenKey = 'auth_token';

  login() {
      localStorage.setItem(this.tokenKey, 'mock-token-123');
  }

  logout(){
    localStorage.removeItem(this.tokenKey);
  }

  isLoggedIn() {
    return !!localStorage.getItem(this.tokenKey);
  }

}
