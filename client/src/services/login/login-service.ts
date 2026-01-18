import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { UsuarioRequest } from '../../models/users';
import { filter, map, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoginService {

  private baseUrl = environment.apiUrl
  private http = inject(HttpClient)

  private authorizationKey = 'authorizedUser'

  userAuthorized = signal<boolean>(this.userIsAuthorized())

  authorizeUser() {
    localStorage.setItem(this.authorizationKey, 'true')
    this.userAuthorized.set(true)
  }

  unauthorizeUser() {
    localStorage.setItem(this.authorizationKey, 'false')
    this.userAuthorized.set(false)
  }

  userIsAuthorized(): boolean {
    return localStorage.getItem(this.authorizationKey) === 'true'
  }

  login(userRequest: UsuarioRequest) : Observable<number>
  {
    return this.http.post(`${this.baseUrl}/login`,
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
