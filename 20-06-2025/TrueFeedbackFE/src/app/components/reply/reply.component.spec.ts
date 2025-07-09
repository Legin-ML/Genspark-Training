import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReplyComponent } from './reply.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ReplyComponent', () => {
  let component: ReplyComponent;
  let fixture: ComponentFixture<ReplyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReplyComponent, HttpClientTestingModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReplyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
