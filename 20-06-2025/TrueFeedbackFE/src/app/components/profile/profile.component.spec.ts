import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfileComponent } from './profile.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../services/auth/auth.service';
import { UserService } from '../../services/user/user.service';
import { of, throwError } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let authServiceMock: any;
  let userServiceMock: any;

  beforeEach(async () => {
    authServiceMock = {
      getUserId: jasmine.createSpy().and.returnValue('test-user-id'),
      getUserRole: jasmine.createSpy().and.returnValue('Admin')
    };

    userServiceMock = {
      get: jasmine.createSpy().and.returnValue(of({
        id: 'test-user-id',
        userName: 'TestUser',
        email: 'test@example.com'
      }))
    };

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ReactiveFormsModule, ProfileComponent],
      providers: [
        MessageService,
        { provide: AuthService, useValue: authServiceMock },
        { provide: UserService, useValue: userServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load user and role on init', () => {
    expect(authServiceMock.getUserId).toHaveBeenCalled();
    expect(userServiceMock.get).toHaveBeenCalledWith('test-user-id');
    expect(component.user.userName).toEqual('TestUser');
    expect(component.role).toEqual('Admin');
  });

  it('should open password change dialog and reset form', () => {
    component.passwordForm.get('newPassword')?.setValue('oldpass');
    component.openChangePassword();
    expect(component.showPasswordDialog).toBeTrue();
    expect(component.passwordForm.get('newPassword')?.value).toBeNull(); 
  });

  it('should not proceed with password change if form invalid', () => {
    component.passwordForm.get('newPassword')?.setValue('');
    component.changePassword();
    expect(component.passwordForm.invalid).toBeTrue();
  });

  it('should handle user load error', () => {
    userServiceMock.get.and.returnValue(throwError(() => new Error('fail')));
    component.ngOnInit();
    expect(userServiceMock.get).toHaveBeenCalled();
  });
});
