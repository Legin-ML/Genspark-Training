import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecipeService } from '../services/recipe.service';

@Component({
  selector: 'app-recipe-view',
  imports: [],
  templateUrl: './recipe-view.html',
  styleUrl: './recipe-view.css'
})
export class RecipeView implements OnInit{
  recipe: any;
  loading = true;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private recipeService: RecipeService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.recipeService.getRecipe(id).subscribe({
        next: (data) => {
          this.recipe = data;
          this.loading = false;
        },
        error: (error) => {
          this.errorMessage = error.message || 'Something went wrong';
          this.loading = false;
        }
      });
    }
  }
}


