import { Component } from '@angular/core';
import { VideoService } from '../../services/video.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-upload-video',
  imports: [FormsModule, CommonModule],
  templateUrl: './upload-video.component.html',
  styleUrls: ['./upload-video.component.css']
})
export class UploadVideoComponent {
  title = '';
  description = '';
  file: File | null = null;
  uploading = false;

  constructor(private videoService: VideoService, private router: Router) {}

  onFileChange(event: any) {
    this.file = event.target.files[0];
  }

  upload() {
    if (!this.file) return;

    this.uploading = true;

    const formData = new FormData();
    formData.append('Title', this.title);
    formData.append('Description', this.description);
    formData.append('File', this.file);

    this.videoService.uploadVideo(formData).subscribe({
      next: () => {
        this.uploading = false;
        alert('Upload successful!');
        this.router.navigate(['/']);
      },
      error: err => {
        this.uploading = false;
        console.error(err);
        alert('Upload failed. Please try again.');
      }
    });
  }
}
