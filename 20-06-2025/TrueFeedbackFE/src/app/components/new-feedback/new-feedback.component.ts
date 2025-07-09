import { Component } from '@angular/core';
import { TextareaModule } from 'primeng/textarea';
import { RatingModule } from 'primeng/rating';
import { ButtonModule } from 'primeng/button';
import {PanelModule} from 'primeng/panel';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NewFeedbackModel } from '../../models/NewFeedbackModel';
import { FeedbackService } from '../../services/feedback/feedback.service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';


@Component({
  selector: 'app-new-feedback',
  imports: [TextareaModule, RatingModule,ButtonModule, ReactiveFormsModule, PanelModule],
  templateUrl: './new-feedback.component.html',
  styleUrl: './new-feedback.component.css'
})
export class NewFeedbackComponent {

    constructor(private fb: FormBuilder, private feedbackService : FeedbackService, private router : Router,
      private messageService :  MessageService
    ){

  }

newFeedbackForm!: FormGroup<any>;

ngOnInit() : void {
 this.newFeedbackForm = this.fb.group({
      message: ['', Validators.required],
      rating: ['', Validators.required]
    });
  }

  onSubmit() {

    if (this.newFeedbackForm.invalid) return;

    const newFeedbackData : NewFeedbackModel = this.newFeedbackForm.value;

    this.feedbackService.create(newFeedbackData).subscribe({
      next: (res) => {
        console.log('feedback created')
        
        this.router.navigate(['/feedbacks'])
      },
      error: (res) => {
          this.messageService.add({
        severity: 'error',
        summary: 'Invalid User',
        detail: 'Please try logging out and in again',
        life: 4000
      });
      }
    })

  }




}
