import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LiveNotificationsComponent } from './live-notifications.component';

describe('LiveNotificationsComponent', () => {
  let component: LiveNotificationsComponent;
  let fixture: ComponentFixture<LiveNotificationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LiveNotificationsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LiveNotificationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
