import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserList } from "./user-list/user-list";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, UserList],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'userdb';
}
