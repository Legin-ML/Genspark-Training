import { Component } from '@angular/core';
import {MenubarModule} from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-navigation',
  imports: [CommonModule, MenubarModule, ButtonModule, AvatarModule, RouterModule],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent {
    items = [
      {label: 'Home', routerLink: '/'},
      {label: 'Feedbacks', routerLink: '/feedbacks'},
      {label: 'New Feedback', routerLink: '/new-feedback'}
    ]
   constructor(public authService: AuthService, private router: Router) {}

  handleLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
