import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, combineLatest, debounceTime, distinctUntilChanged, map, Observable, startWith } from 'rxjs';
import { User } from '../models/User';
import { Store } from '@ngrx/store';
import { selectAllUsers, selectError, selectIsLoading } from '../ngrx/user.selector';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { addUser } from '../ngrx/user.actions';
import { bannedWordsValidator } from '../validations/banned-words.validator';
import { passwordStrengthValidator } from '../validations/password-strength.validator';
import { matchPasswordValidator } from '../validations/match-password.validator';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserList implements OnInit {

  userAddForm!: FormGroup;
  filterForm!: FormGroup;

  users$: Observable<User[]>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  filteredUsers$: Observable<User[]>;

  roles = ['Admin', 'User', 'Guest'];

  constructor(private fb: FormBuilder, private store: Store) {
    this.users$ = this.store.select(selectAllUsers);
    this.loading$ = this.store.select(selectIsLoading);
    this.error$ = this.store.select(selectError);
    this.filteredUsers$ = this.users$;
  }

  ngOnInit(): void {
    this.userAddForm = this.fb.group({
      username: ['', [Validators.required, bannedWordsValidator(['admin', 'root'])]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, passwordStrengthValidator]],
      confirmPassword: ['', Validators.required],
      role: ['', Validators.required],
    }, { validators: matchPasswordValidator });

    this.filterForm = this.fb.group({
      search: [''],
      role: ['']
    });

    const search$ = this.filterForm.get('search')!.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      distinctUntilChanged()
    );

    const role$ = this.filterForm.get('role')!.valueChanges.pipe(
      startWith('')
    );

    this.filteredUsers$ = combineLatest([this.users$, search$, role$]).pipe(
      map(([users, search, role]) => {
        const searchLower = search.toLowerCase();
        return users.filter(user =>
          (user.username.toLowerCase().includes(searchLower) || user.role.toLowerCase().includes(searchLower)) 
        );
      })
    );
  }

  addUser(): void {
    if (this.userAddForm.invalid) {
      return;
    }
    const { username, email, password, role } = this.userAddForm.value;
    const newUser: User = { username, email, password, role };
    this.store.dispatch(addUser({ user: newUser }));
    this.userAddForm.reset();
  }

}
