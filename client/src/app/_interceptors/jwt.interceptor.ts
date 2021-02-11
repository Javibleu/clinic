import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  // inject AccountService because we are storing token there as part of currentuser$
  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User; // holds our current user

    // subscribe using take just to taking 1 to unsuscribe -> pipe(take(1)) takes 1 and unsuscribe after
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);

    // if is not null
    if (currentUser) {
      request = request.clone({ // clone request and add our authentication header
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`
        }
      });
    }
    return next.handle(request); // returns the clone with auth added
  }
}
