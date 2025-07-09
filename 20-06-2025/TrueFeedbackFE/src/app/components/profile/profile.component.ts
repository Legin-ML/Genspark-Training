import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { UserService } from '../../services/user/user.service';
import { MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { UserModel } from '../../models/UserModel';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  imports: [ DialogModule, ReactiveFormsModule, ButtonModule, InputTextModule],
})
export class ProfileComponent implements OnInit {
  user!: UserModel;
  role! : string;
  showPasswordDialog = false;
  passwordForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
     const userId = this.authService.getUserId();
  if (!userId) return;

  this.userService.get(userId).subscribe({
    next: (user) => {
      this.user = user;
    },
    error: (err) => {
      console.error('Failed to fetch user:', err);
    }
  });

  this.role = this.authService.getUserRole() ?? ''

    this.passwordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  openChangePassword() {
    this.passwordForm.reset();
    this.showPasswordDialog = true;
  }

  changePassword() {
    if (this.passwordForm.invalid) return;

    // this.userService.changePassword(this.passwordForm.value.newPassword).subscribe({
    //   next: () => {
    //     this.messageService.add({
    //       severity: 'success',
    //       summary: 'Password Changed',
    //       detail: 'Your password has been updated.',
    //     });
    //     this.showPasswordDialog = false;
    //   },
    //   error: (err) => {
    //     this.messageService.add({
    //       severity: 'error',
    //       summary: 'Error',
    //       detail: err?.error?.message || 'Failed to change password.',
    //     });
    //   },
    // });
  }
}
