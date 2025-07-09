import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';

import { FeedbackModel } from '../../models/FeedbackModel';
import { FeedbackService } from '../../services/feedback/feedback.service';
import { UserService } from '../../services/user/user.service';
import { FeedbackDetailComponent } from '../feedback-detail/feedback-detail.component';
import { LazyLoadEvent } from 'primeng/api';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-feedback-list',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    DialogModule,
    InputTextModule,
    MultiSelectModule,
    FormsModule,
    FeedbackDetailComponent,
  ],
  templateUrl: './feedback-list.component.html',
  styleUrls: ['./feedback-list.component.css'],
})
export class FeedbackListComponent {
  allFeedbacks: FeedbackModel[] = [];
  feedbacks: FeedbackModel[] = [];
  totalFeedbacks = 0;
  loading = false;
  pageSize = 10;

  selectedFeedback: FeedbackModel | null = null;
  dialogVisible = false;

  searchString:string="";

  sortField = '';
  sortOrder = 1;
  filters: any = {};
  ratingOptions = Array.from({ length: 5 }, (_, i) => ({
    label: `${i + 1} Star`,
    value: i + 1,
  }));

  private globalFilter$ = new Subject<string>();

  constructor(
    private feedbackService: FeedbackService,
    private userService: UserService
  ) {
    this.globalFilter$.pipe(debounceTime(300)).subscribe((value) => {
      this.filters['global'] = { value, matchMode: 'contains' };
      this.reload();
    });
  }

  ngOnInit() {
    this.loading = true;
    this.feedbackService.getAllPaginated(1, 999999).subscribe((all) => {
      this.allFeedbacks = all.items;
      this.totalFeedbacks = all.totalCount;
      this.loading = false;
      this.loadFeedbacks({ first: 0, rows: this.pageSize });
      this.allFeedbacks.forEach((fb) => {
        if (fb.userId) {
          this.userService.get(fb.userId).subscribe((u) => (fb.username = u.userName));
        }
      });
    });
  }

  onSelect(fb: FeedbackModel) {
    this.selectedFeedback = fb;
    this.dialogVisible = true;
  }

  onGlobalFilter() {
    this.globalFilter$.next(this.searchString);
  }

  onRatingFilter(values: number[]) {
    if (values && values.length) {
      this.filters['rating'] = { value: values, matchMode: 'in' };
    } else {
      delete this.filters['rating'];
    }
    this.reload();
  }

  loadFeedbacks(event: TableLazyLoadEvent) {
    const start = event.first ?? 0;
    const rows = event.rows ?? this.pageSize;
    this.sortField = Array.isArray(event.sortField)
  ? event.sortField[0]
  : event.sortField ?? '';
    this.sortOrder = event.sortOrder ?? 1;

    let data = [...this.allFeedbacks];

    const gf = this.filters['global']?.value;
    if (gf) {
      const term = gf.toLowerCase();
      data = data.filter((fb) =>
        [fb.username, fb.message, fb.reply]
          .filter(Boolean)
          .some((s) => s!.toLowerCase().includes(term))
      );
    }
    const rv = this.filters['rating']?.value;
    if (Array.isArray(rv) && rv.length) {
      data = data.filter((fb) => rv.includes(fb.rating!));
    }

    if (this.sortField) {
      data.sort((a: any, b: any) => {
        const valA = a[this.sortField];
        const valB = b[this.sortField];
        return valA == null || valB == null
          ? (valA == null ? -1 : 1) * this.sortOrder
          : (valA < valB ? -1 : 1) * this.sortOrder;
      });
    }

    this.totalFeedbacks = data.length;
    this.feedbacks = data.slice(start, start + rows);
  }

  reload() {
    this.loadFeedbacks({
      first: 0,
      rows: this.pageSize,
      sortField: this.sortField,
      sortOrder: this.sortOrder,
    } as LazyLoadEvent);
  }

  onFeedbackUpdated() {
  if (!this.selectedFeedback) return;

  this.dialogVisible = false;

  this.feedbackService.get(this.selectedFeedback.id!).subscribe({
    next: (updated) => {
      const indexAll = this.allFeedbacks.findIndex(fb => fb.id === updated.id);
      if (indexAll !== -1) {
        this.allFeedbacks[indexAll] = updated;
      }

      const indexVisible = this.feedbacks.findIndex(fb => fb.id === updated.id);
      if (indexVisible !== -1) {
        this.feedbacks[indexVisible] = updated;
      }

      if (updated.userId) {
        this.userService.get(updated.userId).subscribe(u => {
          updated.username = u.userName;
        });
      }
    },
    error: (err) => {
      console.error('Failed to refresh feedback after update:', err);
    }
  });
}

}
