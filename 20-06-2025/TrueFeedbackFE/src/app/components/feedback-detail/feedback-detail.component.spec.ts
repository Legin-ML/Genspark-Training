import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FeedbackDetailComponent } from './feedback-detail.component';
import { FeedbackService } from '../../services/feedback/feedback.service';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../services/auth/auth.service';
import { of, throwError } from 'rxjs';
import { FeedbackModel } from '../../models/FeedbackModel';
import { ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('FeedbackDetailComponent', () => {
  let component: FeedbackDetailComponent;
  let fixture: ComponentFixture<FeedbackDetailComponent>;
  let mockFeedbackService: jasmine.SpyObj<FeedbackService>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockMessageService: jasmine.SpyObj<MessageService>;

  const mockFeedback: FeedbackModel = {
    id: 1,
    message: 'Test message',
    rating: 4,
    reply: '',
    userId: '123'
  };

  beforeEach(async () => {
    mockFeedbackService = jasmine.createSpyObj('FeedbackService', ['update', 'reply']);
    mockAuthService = jasmine.createSpyObj('AuthService', ['isAdmin', 'getUserId']);
    mockMessageService = jasmine.createSpyObj('MessageService', ['add']);

    await TestBed.configureTestingModule({
      imports: [FeedbackDetailComponent, ReactiveFormsModule],
      providers: [
        { provide: FeedbackService, useValue: mockFeedbackService },
        { provide: AuthService, useValue: mockAuthService },
        { provide: MessageService, useValue: mockMessageService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(FeedbackDetailComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('as Owner', () => {
    beforeEach(() => {
      component.feedback = mockFeedback;
      mockAuthService.getUserId.and.returnValue('123');
      mockAuthService.isAdmin.and.returnValue(false);
      fixture.detectChanges();
      component.ngOnChanges({ feedback: { currentValue: mockFeedback, previousValue: null, firstChange: true, isFirstChange: () => true } });
    });

    it('should build a form with enabled message field', () => {
      expect(component.form.get('message')?.disabled).toBeFalse();
      expect(component.form.get('reply')?.disabled).toBeTrue();
    });

    it('should submit feedback changes', () => {
      component.form.get('message')?.setValue('Updated message');
      mockFeedbackService.update.and.returnValue(of({}));

      component.onSave();

      expect(mockFeedbackService.update).toHaveBeenCalledWith(1, jasmine.objectContaining({
        message: 'Updated message'
      }));
      expect(mockMessageService.add).toHaveBeenCalledWith(jasmine.objectContaining({ summary: 'Success' }));
    });
  });

  describe('as Admin', () => {
    beforeEach(() => {
      component.feedback = mockFeedback;
      mockAuthService.getUserId.and.returnValue('456');
      mockAuthService.isAdmin.and.returnValue(true);
      fixture.detectChanges();
      component.ngOnChanges({ feedback: { currentValue: mockFeedback, previousValue: null, firstChange: true, isFirstChange: () => true } });
    });

    it('should enable the reply field', () => {
      expect(component.form.get('reply')?.disabled).toBeFalse();
    });

    it('should send a reply', () => {
      component.form.get('reply')?.setValue('Thanks for your feedback!');
      mockFeedbackService.reply.and.returnValue(of({}));

      component.onSendReply();

      expect(mockFeedbackService.reply).toHaveBeenCalledWith(1, 'Thanks for your feedback!');
      expect(mockMessageService.add).toHaveBeenCalledWith(jasmine.objectContaining({
        summary: 'Reply Sent'
      }));
    });

    it('should show error if reply fails', () => {
      component.form.get('reply')?.setValue('Reply here');
      mockFeedbackService.reply.and.returnValue(throwError(() => new Error('Fail')));

      component.onSendReply();

      expect(mockMessageService.add).toHaveBeenCalledWith(jasmine.objectContaining({
        summary: 'Reply Failed'
      }));
    });
  });

  it('should disable message field if not owner', () => {
    component.feedback = { ...mockFeedback, userId: 'other-id' };
    mockAuthService.getUserId.and.returnValue('123');
    mockAuthService.isAdmin.and.returnValue(false);

    component.ngOnChanges({ feedback: { currentValue: mockFeedback, previousValue: null, firstChange: true, isFirstChange: () => true } });
    fixture.detectChanges();

    expect(component.form.get('message')?.disabled).toBeTrue();
  });
});
