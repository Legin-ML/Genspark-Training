import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { FeedbackService } from './feedback.service';
import { environment } from '../../environments/environment';
import { FeedbackModel } from '../../models/FeedbackModel';
import { NewFeedbackModel } from '../../models/NewFeedbackModel';

describe('FeedbackService', () => {
  let service: FeedbackService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.apiUrlBase}/feedbacks`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [FeedbackService]
    });
    service = TestBed.inject(FeedbackService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create feedback via POST', () => {
    const newFeedback: NewFeedbackModel = {
      userId: 'user123',
      message: 'Great service!',
      rating: 4
    };

    service.create(newFeedback).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newFeedback);
    req.flush({ success: true });
  });

  it('should update feedback via PUT', () => {
    const id = 123;
    const feedback: FeedbackModel = new FeedbackModel(
      id,
      'user123',
      'Updated feedback message',
      5,
      'Thanks for your feedback!',
      'reply456',
      new Date('2025-07-01T12:00:00Z'),
      'johndoe'
    );

    service.update(id, feedback).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(feedback);
    req.flush({ success: true });
  });

  it('should get all feedbacks via GET', () => {
    const mockFeedbacks: FeedbackModel[] = [
      new FeedbackModel(1, 'user1', 'Feedback 1', 5, 'Reply 1', 'r1', new Date('2025-01-01T10:00:00Z'), 'alice'),
      new FeedbackModel(2, 'user2', 'Feedback 2', 3, undefined, undefined, new Date('2025-02-01T11:00:00Z'), 'bob')
    ];

    service.getAll().subscribe(feedbacks => {
      expect(feedbacks.length).toBe(2);
      expect(feedbacks).toEqual(mockFeedbacks);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockFeedbacks);
  });

  it('should get all paginated feedbacks via GET', () => {
    const page = 1;
    const pageSize = 10;
    const mockResponse = {
      items: [
        new FeedbackModel(3, 'user3', 'Paginated feedback', 4, undefined, undefined, new Date('2025-03-01T09:00:00Z'), 'charlie')
      ],
      totalCount: 100
    };

    service.getAllPaginated(page, pageSize).subscribe(response => {
      expect(response.items.length).toBe(1);
      expect(response.totalCount).toBe(100);
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${apiUrl}?page=1&pageSize=999999`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should send reply via PUT', () => {
    const id = 456;
    const replyText = 'This is a reply';

    service.reply(id, replyText).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}/reply`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual({ reply: replyText });
    req.flush({ success: true });
  });
});
