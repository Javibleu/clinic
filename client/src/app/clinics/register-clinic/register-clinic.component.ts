import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import {  FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {ClinicService} from '../../_services/clinic.service';

@Component({
  selector: 'app-register-clinic',
  templateUrl: './register-clinic.component.html',
  styleUrls: ['./register-clinic.component.css']
})
export class RegisterClinicComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];



  constructor(private clinicService: ClinicService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.intitializeForm();
  }

  intitializeForm(): void {
    this.registerForm = this.fb.group({
      clinicName: ['', Validators.required],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      url: [],
      email: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      healthprovidertype: ['1'] // 1-Clinic 2-Pharmacy
    });
  }

  register(): void {
    this.clinicService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/clinics');
    }, error => {
      this.validationErrors = error;
    });
   }
   cancel(): void {
    this.cancelRegister.emit(false);
  }
}
