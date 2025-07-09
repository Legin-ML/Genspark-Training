// services/feedback.service.ts
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FeedbackModel } from '../../models/FeedbackModel';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { NewFeedbackModel } from '../../models/NewFeedbackModel';

@Injectable({ providedIn: 'root' })
export class FeedbackService {
  private url = `${environment.apiUrlBase}/feedbacks`;

  constructor(private http: HttpClient) {}

  create(feedback: NewFeedbackModel): Observable<any> {
    return this.http.post(this.url, feedback);
  }

  update(id: number, feedback: FeedbackModel): Observable<any> {
    return this.http.put(`${this.url}/${id}`, feedback);
  }

  get(id: number) : Observable<FeedbackModel> {
    return this.http.get<FeedbackModel>(`${this.url}/${id}`)
  }

  getAll(): Observable<FeedbackModel[]> {
    return this.http.get<FeedbackModel[]>(this.url);
  }

  getAllPaginated(page: number, pageSize: number): Observable<{ items: FeedbackModel[], totalCount: number }> {
  return this.http.get<{ items: FeedbackModel[], totalCount: number }>(
    `${this.url}?page=1&pageSize=999999`
  );
}

  delete(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }


  reply(id: number, reply: string): Observable<any> {
    return this.http.put(`${this.url}/${id}/reply`, { reply });
  }
}
