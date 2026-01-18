import { Component, inject, signal } from '@angular/core';
import { UsuarioRequest } from '../../models/users';
import { form, FormField, required } from '@angular/forms/signals';
import { LoginService } from '../../services/login/login-service';
import { Router } from '@angular/router';
import { FloatLabelModule } from 'primeng/floatlabel';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-login',
  imports: [
    FormField,
    FloatLabelModule,
    ButtonModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  loginService: LoginService = inject(LoginService)

  router: Router = inject(Router)

  loginModel = signal<UsuarioRequest>(
    {
      user: '',
      pass: ''
    }
  )

  loginForm = form(this.loginModel, (sechemaPath) => {
    required(sechemaPath.user, {message: 'El usuario es requerido'})
    required(sechemaPath.pass, {message: 'La contraseña es requerida'})
  })

  onSubmit(event: Event) {
    event.preventDefault()
    this.loginService.login(this.loginModel())
    .subscribe(res => {
      if(res == 200) {
        this.loginService.authorizeUser()
        this.router.navigate(['/users'])
      }
    })
  }
}

