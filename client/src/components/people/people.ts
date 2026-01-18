import { Component, inject, signal } from '@angular/core';
import { PeopleService } from '../../services/people/people-service';
import { rxResource } from '@angular/core/rxjs-interop';
import { PersonaRequest } from '../../models/people';
import { email, form, FormField, required } from '@angular/forms/signals';
import { TableModule } from 'primeng/table';
import { FloatLabelModule } from 'primeng/floatlabel';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-people',
  imports: [
    TableModule,
    FormField,
    FloatLabelModule,
    ButtonModule
  ],
  templateUrl: './people.html',
  styleUrl: './people.css',
})
export class People {

  peopleService: PeopleService = inject(PeopleService)

  loadPeople = signal(0)
  peopleResponse = rxResource({
      params: () => ({load: this.loadPeople()}),
      stream: () => this.peopleService.getPeople(),
      defaultValue: []
    });

  peopleModel = signal<PersonaRequest>(
    {
      nombres: '',
      apellidos: '',
      email: '',
      numeroIdentificacion: '',
      tipoIdentificacion: ''
    }
  )

  peopleForm = form(this.peopleModel, (schemaPath) => {
    required(schemaPath.nombres, {message: 'El nombre es requerido'})
    required(schemaPath.apellidos, {message: 'El apellido es requerido'})
    required(schemaPath.email, {message: 'El email es requerido'})
    email(schemaPath.email, {message: 'El email debe tener el formato correcto'})
    required(schemaPath.numeroIdentificacion, {message: 'El numero de identificación es requerido'})
    required(schemaPath.tipoIdentificacion, {message: 'El tipo de identificación es requerido'})
  })

  loadingUserCreate = false;

  constructor(){
  }

  onSubmit(event: Event) {
    event.preventDefault();
    this.peopleService.createPeople(this.peopleModel())
    .subscribe(res => {
      if(res == 200) {
        this.peopleModel.set( {nombres: '', apellidos: '', email: '', numeroIdentificacion: '', tipoIdentificacion: ''})
        this.loadPeople.set(1)
      }
    })
  }
}
