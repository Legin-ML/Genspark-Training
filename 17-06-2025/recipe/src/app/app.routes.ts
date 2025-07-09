import { Routes } from '@angular/router';
import { RecipeList } from './recipe-list/recipe-list';
import { RecipeView } from './recipe-view/recipe-view';
import { Unauthorized } from './unauthorized/unauthorized';
import { AuthGuard } from './guards/LoginGuard';

export const routes: Routes = [
    { path: 'recipe/:id', component: RecipeView, canActivate: [AuthGuard]},
  { path: '', component: RecipeList, canActivate: [AuthGuard]},
  {path: 'unauthorized', component:Unauthorized},
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
