import { Routes } from '@angular/router';
import { Users } from '../components/users/users';
import { People } from '../components/people/people';

export const routes: Routes = [
  {
    path: '',
    title: 'Bienvenidos',
    component: Users
  },
  {
    path: 'users',
    title: 'Usuarios',
    component: Users
  },
  {
    path: 'people',
    title: 'Personas',
    component: People
  }
];
