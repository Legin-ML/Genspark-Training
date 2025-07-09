import { Component, OnDestroy, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { MessageService } from 'primeng/api';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-live-notifications',
  imports: [],
  templateUrl: './live-notifications.component.html',
  styleUrl: './live-notifications.component.css'
})
export class LiveNotificationsComponent implements OnInit, OnDestroy{
  private connection!: signalR.HubConnection;

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apiSignalR)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.on('FeedbackPosted', (data) => {
      this.messageService.add({
        severity: 'info',
        summary: '🗣 New Feedback',
        detail: `${data.rating}⭐: ${data.message}`,
        life: 5000
      });
    });

    this.connection.on('FeedbackReplied', (data) => {
      this.messageService.add({
        severity: 'info',
        summary: '🛠 Admin Replied',
        detail: `${data.reply}`,
        life: 5000
      });
    });

    this.connection.start()
      .then(() => console.log('✅ SignalR connected for toast notifications'))
      .catch(err => console.error('❌ SignalR connection failed', err));
  }

  ngOnDestroy(): void {
    this.connection?.stop();
  }
}
