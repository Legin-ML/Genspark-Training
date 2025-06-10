import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Products } from "./products/products";
import { CustomerDetails } from "./customer-details/customer-details";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Products, CustomerDetails],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'firstApp';
}
