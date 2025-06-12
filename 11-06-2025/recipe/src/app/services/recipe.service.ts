import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root' 
})
export class RecipeService {
  constructor(private http: HttpClient) {}

  getRecipe(id: number = 1): Observable<any> {
    return this.http.get('https://dummyjson.com/recipes/' + id);
  }

  getAllRecipes(): Observable<any[]> {
    return this.http.get<any>('https://dummyjson.com/recipes').pipe(
      map(res => res.recipes), 
      catchError(error => throwError(() => new Error('Failed to load recipes: ' + error)))
    );
  }
}
