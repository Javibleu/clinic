import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Pagination } from 'src/app/models/pagination';
import { UserParams } from 'src/app/models/userParams';


@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];          // property to store an array of members
  pagination: Pagination;     // stores pagination information
  userParams: UserParams;     // stores userparams

  // list to filter
  genderList = [{ value: 'male', display: 'kinesiologist' }, { value: 'female', display: 'FamilyDoctor' }]; // 160 to add filter buttons
  symptomsList = [{ value: 'male', display: 'BackPain' }, { value: 'female', display: 'Cough' }]; // 160 to add filter buttons
  cityList = [{ value: 'oakville', display: 'Oakville' }, { value: 'burlinghton', display: 'Burlinghton' }]; // 160 to add filter buttons

  // inject Memberservice to get members
  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadMembers(); // to render members on initializing phase
  }

  // created on 155-
  loadMembers(): void {
    this.memberService.setUserParams(this.userParams); //update userparams on server
    // get members using service
    this.memberService.getMembers(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
      //console.log(this.members);
      //console.log(this.pagination);
    });
  }

  // 160- To reset filter
  resetFilters(): void {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  // + 156- receives event from <pagination>
  pageChanged(event: any): void {
    this.userParams.pageNumber = event.page;            // updates page number in userParams.pageNumber 
    this.memberService.setUserParams(this.userParams);  // ?
    this.loadMembers();                                 // to get the next batch of members
  }
}
