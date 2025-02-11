import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatauthguardGuard implements CanActivate {

  constructor(public router: Router) {

  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (localStorage.getItem('CurrentUser')) {
      return true;
    }

    //this.router.navigate(['/home'], { queryParams: { returUrl: state.url } });
    this.router.navigate(['home']);
    return false;
  }
  
}
