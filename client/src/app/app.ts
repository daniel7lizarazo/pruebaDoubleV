import { Component, inject, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { LoginService } from '../services/login/login-service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MenubarModule,
    ButtonModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('client');
  items: MenuItem[] = []

  router: Router = inject(Router)

  loginService: LoginService = inject(LoginService)

  ngOnInit() {
    this.items = [
      {
        label: 'Usuarios',
        routerLink: '/users'
      },
      {
        label: 'Personas',
        routerLink: '/people'
      }
    ]
  }

  logout(event: Event) {
    this.loginService.unauthorizeUser()
    this.router.navigate(['/login'])
  }
}
