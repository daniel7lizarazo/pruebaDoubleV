import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { filter, map, Observable, take } from 'rxjs';
import { PersonaRequest, PersonaResponse } from '../../models/people';

@Injectable({
  providedIn: 'root',
})
export class PeopleService {

  private baseUrl = environment.apiUrl
  private http = inject(HttpClient)

  getPeople() : Observable<PersonaResponse[]>
  {
    return this.http.get<PersonaResponse[]>(`${this.baseUrl}/people`);
  }

  createPeople(peopleRequest: PersonaRequest) : Observable<number>
  {
    return this.http.post(`${this.baseUrl}/people`,
      peopleRequest,
      { reportProgress: true, observe: 'events', }
    )
    .pipe(
      filter((res) => res.type == HttpEventType.Response),
      map(res => res.status),
      take(1),
    )
  }
}
