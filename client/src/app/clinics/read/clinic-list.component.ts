import { Component, OnInit } from '@angular/core';
import { Clinic } from 'src/app/models/Clinic';
import { ClinicService } from 'src/app/_services/clinic.service';

@Component({
  selector: 'app-clinic-list',
  templateUrl: './clinic-list.component.html',
  styleUrls: ['./clinic-list.component.css']
})


export class ClinicListComponent implements OnInit {
  clinics: Clinic[];
  displayedColumns: string[] = ['position', 'name', 'phone', 'address', 'webpage', 'account'];
  datasource;

  constructor(private clinicService: ClinicService) { }

  ngOnInit(): void {
    this.loadClinics(); // to render members on init stage
  }

  loadClinics() {
    this.clinicService.getClinics().subscribe(response => {
      //console.log(response);
      this.clinics = response;
      this.datasource = response;
    });
  }
}

