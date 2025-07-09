import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewFeedbackComponent } from './new-feedback.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('NewFeedbackComponent', () => {
  let component: NewFeedbackComponent;
  let fixture: ComponentFixture<NewFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewFeedbackComponent, HttpClientTestingModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
