import { Routes } from '@angular/router';
import { VideoListComponent } from './components/video-list/video-list.component';
import { UploadVideoComponent } from './components/upload-video/upload-video.component';
import { VideoViewComponent } from './components/video-view/video-view.component';

export const routes: Routes = [
    { path: 'upload', component: UploadVideoComponent },
    { path: 'videos', component: VideoListComponent },
    { path: 'videos/:id', component: VideoViewComponent },
    { path: '', redirectTo: '/videos', pathMatch: 'full' }, 
    { path: '**', redirectTo: '/videos' } 
];
