import { inject, Injectable } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { LoginService } from '../services/login/login-service';

export const authorizationGuard: CanActivateFn = (route, state) => {
  let loginService: LoginService = inject(LoginService)
  let router: Router = inject(Router)

  if(route.url.length <= 0)
  {
    return router.createUrlTree(['/login'])
  }

  let path = route.url[0].path

  if(path.includes('login'))
  {
    if(loginService.userIsAuthorized())
    {
      return router.createUrlTree(['/users'])
    }
    return true
  }
  if(!loginService.userIsAuthorized())
  {
    return router.createUrlTree(['/login'])
  }
  return true;
};
