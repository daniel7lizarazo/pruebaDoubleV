import { Routes } from '@angular/router';
import { Users } from '../components/users/users';
import { People } from '../components/people/people';
import { Login } from '../components/login/login';
import { authorizationGuard } from '../guards/authorization-guard';

export const routes: Routes = [
  {
    path: '',
    title: 'Bienvenidos',
    component: Login,
    canActivate: [authorizationGuard]
  },
  {
    path: 'users',
    title: 'Usuarios',
    component: Users,
    canActivate: [authorizationGuard]
  },
  {
    path: 'people',
    title: 'Personas',
    component: People,
    canActivate: [authorizationGuard]
  },
  {
    path: 'login',
    title: 'Login',
    component: Login,
    canActivate: [authorizationGuard]
  }
];
