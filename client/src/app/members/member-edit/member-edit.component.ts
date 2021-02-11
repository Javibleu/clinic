import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;  // reference to form tag in html <form #editForm="ngForm">
  member: Member;                           // holds member got from http
  user: User;                               // holds the current user


  // Access to browser events, in this case if the page is unload
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any): void {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  // Injects Accountservice to get currentuser$, MemberService to get http
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(): void {
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
    });
  }

  // Uses members.service/update member
  updateMember(): void {
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success('Profile updated successfully');
      this.editForm.reset(this.member);                     // after edit, reset form
    });
  }
}
