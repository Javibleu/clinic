import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
// 212 Guard to authorize acces to admin component
export class AdminGuard implements CanActivate {
  // 212 + constructor to get access to AccountService.currentUser$ and Toastr
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  // returns and Observable of boolean if currentUser.user ==Admin or Moderator returns true
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user.roles.includes('Admin') || user.roles.includes('Moderator')) {
          return true;
        }
        this.toastr.error('You cannot enter this area');
      })
    );
  }

}
