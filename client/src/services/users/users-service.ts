import { inject, Injectable } from '@angular/core';
import { UsuarioRequest, UsuariosResponse } from '../../models/users';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { filter, map, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsersService {

  private baseUrl = environment.apiUrl
  private http = inject(HttpClient)

  getUsers() : Observable<UsuariosResponse[]>
  {
    return this.http.get<UsuariosResponse[]>(`${this.baseUrl}/users`);
  }

  createUser(userRequest: UsuarioRequest) : Observable<number>
  {
    return this.http.post(`${this.baseUrl}/users`,
      userRequest,
      { reportProgress: true, observe: 'events', }
    )
    .pipe(
      filter((res) => res.type == HttpEventType.Response),
      map(res => res.status),
      take(1),
    )
  }
}
