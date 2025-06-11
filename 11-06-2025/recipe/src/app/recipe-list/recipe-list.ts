import { Component, OnInit, signal } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { RecipeModel } from '../models/recipe';
import { Recipe } from '../recipe/recipe';

const isDev = false;

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipe-list.html',
  styleUrls: ['./recipe-list.css']
})
export class RecipeList implements OnInit {
  recipes = signal<RecipeModel[]>([]);

  constructor(private recipeService: RecipeService) {
  }

  ngOnInit(): void {
    this.recipeService.getAllRecipes().subscribe({
      next: (data: any) => {
        const result = isDev ? (data as RecipeModel[]) : [];
        this.recipes.set(result); 
      },
      error: (err) => {
        console.error('Error loading recipes', err);
      }
    });
  }
}
