import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customer-details',
  imports: [FormsModule],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css'
})
export class CustomerDetails {
like() {
  this.likes_class = "bi bi-hand-thumbs-up-fill"
  this.likes += 1
  this.likes_class = "bi bi-hand-thumbs-up"
}
dislike() {
  this.dislikes_class = "bi bi-hand-thumbs-down-fill"
 this.dislikes += 1
 this.dislikes_class = "bi bi-hand-thumbs-down"
}
name: string;
email: string;
likes: number;
dislikes: number;
likes_class: string;
dislikes_class: string;

constructor() {
  this.name = "Jane Doe"
  this.email = "janedoe@example.com"
  this.likes = 0
  this.dislikes = 0
  this.likes_class = "bi bi-hand-thumbs-up"
  this.dislikes_class = "bi bi-hand-thumbs-down"
}

}
