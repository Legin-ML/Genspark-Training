import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { UserModel } from '../models/usermodel';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {}

  getUser(id: number = 1): Observable<UserModel> {
    return this.http.get<UserModel>('https://dummyjson.com/users/' + id);
  }

  getAllUsers(): Observable<UserModel[]> {
    return this.http.get<any>('https://dummyjson.com/users').pipe(
      map(res => {
        return res.users.map((u: any) => ({
          id: u.id,
          firstName: u.firstName,
          lastName: u.lastName,
          age: u.age,
          gender: u.gender,
          state: u.state,
          role: u.company?.title || 'Unknown'
        }));
      }),
      catchError(error =>
        throwError(() => new Error('Failed to load users: ' + error))
      )
    );
  }

}
