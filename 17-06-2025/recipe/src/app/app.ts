import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { RecipeList } from "./recipe-list/recipe-list";
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'recipe';

  constructor(public authService: AuthService, private router: Router) {}

  login() {
    this.authService.login()
    this.router.navigate(['/recipe']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['']);
  }
}
