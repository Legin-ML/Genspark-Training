import { Component } from '@angular/core';
import { VideoService } from '../../services/video.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-video-list',
  imports: [CommonModule],
  templateUrl: './video-list.component.html',
  styleUrl: './video-list.component.css'
})
export class VideoListComponent {
    videos: any[] = [];

  constructor(private videoService: VideoService, private router : Router) {}

  ngOnInit(): void {
    this.videoService.getVideos().subscribe(videos => this.videos = videos);
  }

  getVideoSrc(video: any) {
    return video.blobUrl ?? this.videoService.getStreamUrl(video.id);
  }

  goToVideo(id: number) {
  this.router.navigate(['/videos', id]);
}
}
