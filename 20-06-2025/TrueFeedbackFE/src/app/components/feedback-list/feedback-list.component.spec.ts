import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackListComponent } from './feedback-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('FeedbackListComponent', () => {
  let component: FeedbackListComponent;
  let fixture: ComponentFixture<FeedbackListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeedbackListComponent, HttpClientTestingModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FeedbackListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
