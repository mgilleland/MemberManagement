import { Component, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MemberService } from "./shared/member.service";

@Component({
  templateUrl: './member-delete.component.html'
})

export class MemberDeleteComponent {

  @Input() memberId: number;
  @Input() userName: string;
  @Output() reloadMemberList = new EventEmitter();
  dataError: boolean = false;
  submitInProgress: boolean = false;

  constructor(private memberService: MemberService, public activeModal: NgbActiveModal) { }

  deleteMember() {
    this.submitInProgress = true;

    this.memberService.deleteMember(this.memberId).subscribe(
      () => {
        this.activeModal.close();
        this.reloadMemberList.emit();
      },
      error => {
        this.dataError = true;
        this.submitInProgress = false;
        console.log("Error calling memberService.deleteMember()");
        console.log(error);
      }
    );
  }
}
