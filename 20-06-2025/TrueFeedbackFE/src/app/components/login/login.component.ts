import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext'
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TabsModule } from 'primeng/tabs';
import { SelectModule } from 'primeng/select';
import { LoginService } from '../../services/login/login.service';
import { LoginModel } from '../../models/LoginModel';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SignUpModel } from '../../models/SignUpModel';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-login',
  imports: [CommonModule, InputTextModule, TabsModule,ButtonModule, CardModule, SelectModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  roles : string[] = []

  activeTabIndex = 1;

  loginUserForm!: FormGroup
registerUserForm!: FormGroup;

  constructor(private fb: FormBuilder, private loginService: LoginService, private router: Router, private messageService : MessageService){

  }

  ngOnInit() : void {
    this.roles = ['User', 'Admin']
    this.loginUserForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]]
    })

    this.registerUserForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(12)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      roleName: ['', Validators.required],
    })
  }

   onLogin() {
    if (this.loginUserForm.invalid) return;

    const loginData: LoginModel = this.loginUserForm.value;

    this.loginService.requestLogin(loginData).subscribe({
      next: (res) => {
        console.log('Login success:', res);
        this.router.navigate(['/feedbacks'])
      },
      error: (err) => {
        console.error('Login failed:', err);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: `Invalid Credentials`, life: 3000 })

      }
    });
  }

  onRegister() {
    if (this.registerUserForm.invalid) return;

    const signupData : SignUpModel = this.registerUserForm.value;

    this.loginService.requestSignUp(signupData).subscribe({
      next: (res) =>{
        console.log("User signed up successfully")
        this.router.navigate(['/login'])
      },
      error: (err) => {
        console.log(err)
        this.messageService.add({ severity: 'error', summary: 'Error', detail: `Email/Username already in use`, life: 3000 })
      }
    })
  }

}
