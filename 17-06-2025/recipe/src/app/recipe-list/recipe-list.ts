import { Component, HostListener, OnInit, signal } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { RecipeModel } from '../models/recipe';
import { Recipe } from '../recipe/recipe';
import { debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

const isDev = false;

@Component({
  selector: 'app-recipes',
  imports: [Recipe, FormsModule],
  templateUrl: './recipe-list.html',
  styleUrls: ['./recipe-list.css']
})
export class RecipeList implements OnInit {
  recipes:RecipeModel[] = [];
  searchString:string="";
  searchSubject = new Subject<string>();
  loading:boolean = false;
  limit=10;
  skip=0;
  total =0;

  constructor(private recipeService: RecipeService, private router: Router) {
  }

   handleSearch(){
    this.searchSubject.next(this.searchString);
  }

  goToRecipe(id: number) {
  this.router.navigate(['/recipe', id]);
}
  

  ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      tap(() => {
        this.loading = true;
        this.skip = 0; 
      }),
      switchMap(query =>
        this.recipeService.getRecipeSearchResults(query, this.limit, this.skip)
      ),
      tap(() => this.loading = false)
    ).subscribe({
      next: (data: any) => {
        this.recipes = data.recipes as RecipeModel[];
        this.total = data.total;
        console.log('Total recipes:', this.total);
      },
      error: err => {
        console.error('Search failed:', err);
        this.loading = false;
      }
    });
      this.searchSubject.next('')
  }

   @HostListener('window:scroll',[])
  onScroll():void
  {

    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight-100;
    if(scrollPosition>=threshold && this.recipes?.length<this.total)
    {
      console.log(scrollPosition);
      console.log(threshold)
      
      this.loadMore();
    }
  }
  loadMore(){
    this.loading = true;
    this.skip += this.limit;
    this.recipeService.getRecipeSearchResults(this.searchString,this.limit,this.skip)
        .subscribe({
          next:(data:any)=>{
            this.recipes = [...this.recipes, ...data.recipes];
            this.loading = false;
          }
        })
  }
}
