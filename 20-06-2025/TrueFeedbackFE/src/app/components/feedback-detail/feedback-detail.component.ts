import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TextareaModule } from 'primeng/textarea';
import { RatingModule } from 'primeng/rating';
import { ButtonModule } from 'primeng/button';
import {ToastModule} from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { FeedbackModel } from '../../models/FeedbackModel';
import { FeedbackService } from '../../services/feedback/feedback.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-feedback-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TextareaModule,
    RatingModule,
    ButtonModule,
    ToastModule
  ],
  templateUrl: './feedback-detail.component.html',
  styleUrl: './feedback-detail.component.css'
})
export class FeedbackDetailComponent implements OnChanges {
  @Input() feedback!: FeedbackModel;
  isAdmin: boolean = false;
  isOwner: boolean = false;

  @Output() updated = new EventEmitter<void>();

  form!: FormGroup;

  constructor(private fb: FormBuilder, 
    private feedbackService : FeedbackService, 
    private messageService : MessageService,
    private authService : AuthService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['feedback'] && this.feedback) {
      this.isAdmin = this.authService.isAdmin();
      this.isOwner = this.authService.getUserId() === this.feedback.userId;
      this.buildForm();
    }
    this.isAdmin = this.authService.isAdmin();
    this.isOwner = this.authService.getUserId() === this.feedback.userId;
  }

  buildForm() {
    this.form = this.fb.group({
      message: [{ value: this.feedback.message || '', disabled: !this.isOwner }, Validators.required],
      rating: [{ value: this.feedback.rating ?? 0, disabled: true }],
      reply: [{ value: this.feedback.reply ?? '', disabled: !this.isAdmin }]
    });
  }

  onSave() {
    if (this.form.invalid) return;

    const updatedFeedback: FeedbackModel = {
      ...this.feedback,
      message: this.form.value.message,
      rating: this.form.value.rating
    };
    this.feedbackService.update(this.feedback.id ?? 0, updatedFeedback).subscribe({
    next: () => {
      console.log('Feedback updated successfully')
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Feedback updated successfully', life: 3000 })
      this.updated.emit()
    },
    error: (err) => console.error('Error updating feedback', err)
  });
  }

  onSendReply() {
  if (!this.feedback.id || this.form.get('reply')?.invalid) return;

  const reply = this.form.get('reply')?.value;

  this.feedbackService.reply(this.feedback.id, reply).subscribe({
    next: () => {
      this.messageService.add({
        severity: 'success',
        summary: 'Reply Sent',
        detail: 'Reply successfully sent to user',
        life: 3000
      });
      this.updated.emit(); 
    },
    error: (err) => {
      console.error('Error sending reply', err);
      this.messageService.add({
        severity: 'error',
        summary: 'Reply Failed',
        detail: 'Could not send reply',
        life: 3000
      });
    }
  });
}

onDelete() {
  if (!this.feedback.id) return;

  if (confirm('Are you sure you want to delete this feedback?')) {
    this.feedbackService.delete(this.feedback.id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Deleted',
          detail: 'Feedback deleted successfully',
          life: 3000
        });
        this.updated.emit();
      },
      error: (err) => {
        console.error('Error deleting feedback', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Delete Failed',
          detail: 'Could not delete feedback',
          life: 3000
        });
      }
    });
  }
}

}
