import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  // 216 to receive something from our component, EventEmitter to emit roles from this component
  @Input() updateSelectedRoles = new EventEmitter();
  user: User; // tho get the user info
  roles: any[]; // bring the roles

  // 215-Injects modal reference
  constructor(public bsModalRef: BsModalRef) { }
  ngOnInit(): void {
  }

  updateRoles(): void {
    this.updateSelectedRoles.emit(this.roles);  // to emit the list of roles
    this.bsModalRef.hide();                     // to hide modal
  }
}
