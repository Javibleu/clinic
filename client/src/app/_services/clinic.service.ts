import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Clinic } from '../models/Clinic';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {
  baseUrl = environment.apiUrl;
  clinics: Clinic[] = [];


  constructor(private http: HttpClient) { }

  getClinics(): Observable<Clinic[]> {

     return this.http.get<Partial<Clinic[]>>(this.baseUrl + 'clinic');
  }

  register(clinic: any) {
    return this.http.post(this.baseUrl + 'clinic/register', clinic);
  }


}
