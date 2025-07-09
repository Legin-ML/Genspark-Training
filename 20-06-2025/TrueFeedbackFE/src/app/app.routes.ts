import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { FeedbackListComponent } from './components/feedback-list/feedback-list.component';
import { LoginComponent } from './components/login/login.component';
import { NewFeedbackComponent } from './components/new-feedback/new-feedback.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AuthGuard } from './guards/authguard';

export const routes: Routes = [
    {path: '', component:HomeComponent},
    {path: 'feedbacks', component:FeedbackListComponent},
    {path: 'login', component:LoginComponent},
    {path: 'new-feedback', component:NewFeedbackComponent, canActivate:[AuthGuard]},
    {path: 'profile', component:ProfileComponent, canActivate:[AuthGuard]}
];
