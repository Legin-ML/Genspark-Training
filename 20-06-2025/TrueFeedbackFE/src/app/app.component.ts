import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavigationComponent } from "./components/navigation/navigation.component";
import { ToastModule } from 'primeng/toast';
import { LiveNotificationsComponent } from "./components/live-notifications/live-notifications.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavigationComponent, ToastModule, LiveNotificationsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TrueFeedbackFE';
}
