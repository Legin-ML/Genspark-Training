import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class VideoService {
  private apiUrl = 'http://localhost:5092/api/videos';

  constructor(private http: HttpClient) {}

  uploadVideo(formData: FormData) {
    return this.http.post(`${this.apiUrl}/upload`, formData);
  }

  getVideos() {
    return this.http.get<any[]>(this.apiUrl);
  }

  getStreamUrl(id: number) {
    return `${this.apiUrl}/${id}/stream`;
  }

  getVideoById(id: number) {
    return `${this.apiUrl}/${id}`
  }
}
