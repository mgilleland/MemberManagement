import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DataTablesModule, DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs/Subject';
import { MemberDetailsComponent } from './member-details.component';
import { MemberDeleteComponent } from './member-delete.component';
import { IMember } from "./shared/member.model";
import { MemberService } from "./shared/member.service";
import { InputModeType } from '../input-mode-type.enum';

@Component({
  selector: 'members-list',
  templateUrl: './members-list.component.html'
})
export class MembersListComponent implements OnInit {
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;

  members: IMember[];
  dataError: boolean = false;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();

  constructor(private memberService: MemberService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.dataError = false;

    this.memberService.getMembers().subscribe(m => {
      this.members = m;
      this.dtTrigger.next();
    }, error => {
      this.dataError = true;
      console.log("Error calling memberService.getMembers()");
      console.log(error);
    });
  }

  // Opens the Member Details in a popup in Add mode
  addNewMember() {
    const modalRef = this.modalService.open(MemberDetailsComponent, { centered: true });
    modalRef.componentInstance.inputMode = InputModeType.Add;

    // Subscribe to the reload event
    modalRef.componentInstance.reloadMemberList.subscribe(() => this.reloadList());
  }

  // Opens the Member Details in a popup in Edit mode
  editMember(id: number) {
    const modalRef = this.modalService.open(MemberDetailsComponent, { centered: true });
    modalRef.componentInstance.inputMode = InputModeType.Edit;
    modalRef.componentInstance.memberId = id;

    // Subscribe to the reload event
    modalRef.componentInstance.reloadMemberList.subscribe(() => this.reloadList());
  }

  // Opens the Delete Member Details popup
  deleteMember(id: number, userName: string) {
    const modalRef = this.modalService.open(MemberDeleteComponent, { centered: true });
    modalRef.componentInstance.memberId = id;
    modalRef.componentInstance.userName = userName;

    // Subscribe to the reload event
    modalRef.componentInstance.reloadMemberList.subscribe(() => this.reloadList());
  }

  reloadList() {
    this.memberService.getMembers().subscribe(m => {
      this.members = m;

      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        dtInstance.destroy();
        this.dtTrigger.next();
      });
    });
  }
}
