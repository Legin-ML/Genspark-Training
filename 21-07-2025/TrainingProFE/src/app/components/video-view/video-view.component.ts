import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VideoService } from '../../services/video.service';

@Component({
  selector: 'app-video-view',
  imports: [CommonModule],
  templateUrl: './video-view.component.html',
  styleUrl: './video-view.component.css'
})
export class VideoViewComponent implements OnInit {
    video: any;

  constructor(
    private route: ActivatedRoute,
    private videoService: VideoService
  ) {}

  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id')!;
    this.videoService.getVideos().subscribe(videos => {
      this.video = videos.find(v => v.id === id);
    });
  }

  get videoSrc() { 
    if (!this.video) return '';
    return `http://localhost:5092/api/videos/${this.video.id}/stream`;
  }
}
