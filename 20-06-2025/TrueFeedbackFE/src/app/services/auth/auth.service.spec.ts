import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('AuthService', () => {
  let service: AuthService;

  const mockToken =
    'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.' +
    btoa(JSON.stringify({
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "user-123",
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "user@test.com",
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Admin",
      exp: Math.floor(Date.now() / 1000) + 3600,
      iss: "issuer",
      aud: "aud"
    })) +
    '.signature';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(AuthService);
    
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should detect logged in when token is set', () => {
    localStorage.setItem('auth_token', mockToken);
    expect(service.isLoggedIn()).toBeTrue();
  });

  it('should return user ID from token', () => {
    localStorage.setItem('auth_token', mockToken);
    expect(service.getUserId()).toBe('user-123');
  });

  it('should return user role from token', () => {
    localStorage.setItem('auth_token', mockToken);
    expect(service.getUserRole()).toBe('Admin');
  });

  it('should return true if user is admin', () => {
    localStorage.setItem('auth_token', mockToken);
    expect(service.isAdmin()).toBeTrue();
  });

  it('should logout user by removing token', () => {
    localStorage.setItem('auth_token', mockToken);
    service.logout();
    expect(service.getToken()).toBeNull();
    expect(service.isLoggedIn()).toBeFalse();
  });
});
