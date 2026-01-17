import { inject, Injectable } from '@angular/core';
import { UsuariosResponse } from '../../models/users';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
}
