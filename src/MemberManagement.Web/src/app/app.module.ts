import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DataTablesModule } from 'angular-datatables';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

import { MembersAppComponent } from './members-app.component';
import { MembersListComponent } from './members/members-list.component';
import { MemberDetailsComponent } from './members/member-details.component';
import { MemberDeleteComponent } from './members/member-delete.component';
import { MemberService } from './members/shared/member.service';
import { PhonePipe } from './pipes/phone.pipe';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule.forRoot(),
    DataTablesModule
  ],
  declarations: [
    MembersAppComponent,
    MembersListComponent,
    MemberDetailsComponent,
    MemberDeleteComponent,
    PhonePipe
  ],
  providers: [MemberService, { provide: 'moment', useValue: moment}],
  entryComponents: [
    MemberDetailsComponent,
    MemberDeleteComponent
  ],
  bootstrap: [MembersAppComponent]
})
export class AppModule { }
