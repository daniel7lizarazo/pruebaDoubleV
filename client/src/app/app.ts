import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { PrimeNG } from 'primeng/config'
import { MenubarModule } from 'primeng/menubar';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MenubarModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('client');
  items: MenuItem[] = []

  constructor(private primeng: PrimeNG) {}

  ngOnInit() {
    this.items = [
      {
        label: 'Usuarios',
        icon: 'pi pi-prime',
        routerLink: '/users'
      },
      {
        label: 'Personas',
        icon: 'pi pi-users',
        routerLink: '/people'
      }
    ]
  }
}
