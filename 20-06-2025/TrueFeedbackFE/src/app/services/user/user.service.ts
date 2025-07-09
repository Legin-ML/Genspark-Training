import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { UserModel } from '../../models/UserModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url = `${environment.apiUrlBase}/users`

  constructor(private http: HttpClient) { }

  get(id: string): Observable<UserModel> {
    return this.http.get<any>(`${this.url}/${id}`).pipe(
      map(res => UserModel.fromApi(res))
    );
  }
}
