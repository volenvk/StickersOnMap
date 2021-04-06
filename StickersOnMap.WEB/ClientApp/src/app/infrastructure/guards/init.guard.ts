import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {SettingsService} from "../../services/settings.service";

@Injectable({
  providedIn: 'root'
})
export class InitGuard implements CanActivate {
  constructor(private settingsService: SettingsService, private router: Router) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.settingsService.initialized) {
      return true;
    }

    return this.settingsService.init()
      .pipe(
        map(success => {
          if (!success) {
            return this.router.parseUrl('/error');
          }

          return success;
        })
      );
  }
}
