import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-products',
  imports: [FormsModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products {
products;
likes: number;

constructor(){
  this.likes = 0;
  this.products = [
    { name: 'Phone', price: 12999, image: 'favicon.ico' },
    { name: 'Laptop', price: 59999, image: 'favicon.ico' },
    { name: 'Headphones', price: 399, image: 'favicon.ico' }
  ]
}
cartButtonClicked(){
  this.likes += 1
}

}
