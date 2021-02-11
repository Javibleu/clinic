import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRoles(): Observable<User[]> {
    // 214- Partial because we are only getting some of the user properties
    return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles'); 
  }

  // 217 called by RolesModalComponent receives user and roles to be assigned
  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }
}
