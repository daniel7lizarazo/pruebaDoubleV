import { Component, inject, signal } from '@angular/core';
import { UsersService } from '../../services/users/users-service';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { TableModule } from 'primeng/table';
import { UsuarioRequest } from '../../models/users';
import { form, FormField, required } from '@angular/forms/signals';
import { FloatLabelModule } from 'primeng/floatlabel';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-users',
  imports: [
    TableModule,
    FormField,
    FloatLabelModule,
    ButtonModule
  ],
  templateUrl: './users.html',
  styleUrl: './users.css',
})


export class Users {

  usersService: UsersService = inject(UsersService)

  loadUsers = signal(0)
  usersResponse = rxResource({
      params: () => ({load: this.loadUsers()}),
      stream: () => this.usersService.getUsers(),
      defaultValue: []
    });

  userModel = signal<UsuarioRequest>(
    {
      user: '',
      pass: ''
    }
  )

  loginForm = form(this.userModel, (schemaPath) => {
    required(schemaPath.user, {message: 'El usuario es requerido'})
    required(schemaPath.pass, {message: 'La contraseña es requerida'})
  })

  constructor(){
  }

  onSubmit(event: Event) {
    event.preventDefault();
    this.usersService.createUser(this.userModel())
    .subscribe(res => {
      if(res == 200) {
        this.userModel.set({ user: '', pass: '' })
        this.loadUsers.set(1)
      }
    })
  }
}
