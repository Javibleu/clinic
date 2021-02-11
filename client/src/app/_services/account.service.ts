import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { User } from '../models/user';
import { Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;                             // from environment (angular will use the correct one dev/prod)
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService) { }

  // called by nav register, sends model in body
  login(model: any): Observable<void> {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user: User = response;
        if (user) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }
      })
    );
  }

  register(model: any): Observable<void> {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
         this.setCurrentUser(user);
         this.presence.createHubConnection(user);
        }
      })
    );
  }

  setCurrentUser(user: User): void {
    user.roles = [];                                        // 212-creates a string array to hold roles
    const roles = this.getDecodedToken(user.token).role;    // 212-get token from user pass it to []
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles); // 212-is array? pass it to user.roles, isnt, push string into.
    localStorage.setItem('user', JSON.stringify(user));    // save user in localStorage
    this.currentUserSource.next(user);                    // set currentUser user Object
  }

  logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }

  getDecodedToken(token): any {
    return JSON.parse(atob(token.split('.')[1])); // splits string and takes the second pos array(roles)
  }
}
