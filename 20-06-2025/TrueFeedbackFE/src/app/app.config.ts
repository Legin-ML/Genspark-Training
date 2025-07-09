import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {providePrimeNG} from 'primeng/config';
import Aura from '@primeng/themes/aura'
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { LoginService } from './services/login/login.service';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { FeedbackService } from './services/feedback/feedback.service';
import { AuthService } from './services/auth/auth.service';
import {  reqInterceptorFn } from './utils/interceptor';
import { UserService } from './services/user/user.service';
import { MessageService } from 'primeng/api';


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura
      }
    }),
    provideHttpClient(withInterceptors([reqInterceptorFn])),
    LoginService,
    FeedbackService,
    AuthService,
    UserService,
    MessageService,


  ]
};
