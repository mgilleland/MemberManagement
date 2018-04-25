import { Component, OnInit, Input, Inject, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IMember } from "./shared/member.model";
import { MemberService } from "./shared/member.service";
import { InputModeType } from '../input-mode-type.enum';
import { PhonePipe } from "../pipes/phone.pipe";

import "rxjs/add/operator/debounceTime";

@Component({
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})

export class MemberDetailsComponent implements OnInit {

  @Input() inputMode: InputModeType;
  @Input() memberId: number;
  @Output() reloadMemberList = new EventEmitter();

  member: IMember;
  title: string;
  dataError: boolean = false;
  dataErrorMessage: string;
  fetchingData: boolean = false;
  submitInProgress: boolean = false;
  currentUserName: string = "";
  phonePipe = new PhonePipe();
  datePipe = new DatePipe(navigator.language);

  // Form controls
  memberForm: FormGroup;
  id: FormControl;
  userName: FormControl;
  firstName: FormControl;
  lastName: FormControl;
  email: FormControl;
  phoneNumber: FormControl;
  dateOfBirth: FormControl;

  userNameMessage: string;
  firstNameMessage: string;
  lastNameMessage: string;
  emailMessage: string;
  phoneNumberMessage: string;
  dateOfBirthMessage: string;

  private validationMessages = {
    required: "Required",
    minlength: "Value is too short",
    maxlength: "Value is too long",
    email: "Must be a valid email"
  }

  constructor(private memberService: MemberService, public activeModal: NgbActiveModal, @Inject('moment') private moment) { }

  ngOnInit() {

    // Setup the form controls
    this.id = new FormControl;
    this.userName = new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(12)]);
    this.firstName = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]);
    this.lastName = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]);
    this.email = new FormControl('', [Validators.required, Validators.maxLength(75), Validators.email]);
    this.phoneNumber = new FormControl;
    this.dateOfBirth = new FormControl('', Validators.required);

    this.memberForm = new FormGroup({
      id: this.id,
      userName: this.userName,
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      phoneNumber: this.phoneNumber,
      dateOfBirth: this.dateOfBirth
    });

    // Set the title for adding
    if (this.inputMode === InputModeType.Add) {
      this.title = "Add New Member";
    }

    // Set the title for editing and get the selected member values
    if (this.inputMode === InputModeType.Edit) {
      this.fetchingData = true;

      this.memberService.getMember(this.memberId).subscribe(m => {
        this.member = m;
        this.currentUserName = this.member.userName.toLowerCase();

        this.id.setValue(this.member.id);
        this.userName.setValue(this.member.userName);
        this.firstName.setValue(this.member.firstName);
        this.lastName.setValue(this.member.lastName);
        this.email.setValue(this.member.email);
        if (this.member.phoneNumber) {
          this.phoneNumber.setValue(this.phonePipe.transform(this.member.phoneNumber));
        }
        this.dateOfBirth.setValue(this.datePipe.transform(this.member.dateOfBirth, "dd/MM/yyyy"));
        this.fetchingData = false;
      }, error => {
        this.dataError = true;
        this.fetchingData = false;
        this.dataErrorMessage = "An error occurred getting the Member";
        console.log("Error calling memberService.getMember()");
        console.log(error);
      });

      this.title = "Edit Member";
    }

    this.userName.valueChanges.debounceTime(1000).subscribe((): void => {
      this.userNameMessage = this.setUserNameMessage(this.userName);
    });
    this.firstName.valueChanges.debounceTime(1000).subscribe((): void => {
      this.firstNameMessage = this.setMessage(this.firstName);
    });
    this.lastName.valueChanges.debounceTime(1000).subscribe(() => {
      this.lastNameMessage = this.setMessage(this.lastName);
    });
    this.email.valueChanges.debounceTime(1000).subscribe(() => {
      this.emailMessage = this.setMessage(this.email);
    });
    this.phoneNumber.valueChanges.debounceTime(1000).subscribe(() => {
      this.phoneNumberMessage = this.setPhoneNumberMessage(this.phoneNumber);
    });
    this.dateOfBirth.valueChanges.debounceTime(1000).subscribe(() => {
      this.dateOfBirthMessage = this.setDateOfBirthMessage(this.dateOfBirth);
    });
  }

  // Set generic error logic message
  setMessage(c: AbstractControl): string {
    let message: string = "";

    if ((c.touched || c.dirty) && c.errors) {
      message = Object.keys(c.errors).map(key =>
        this.validationMessages[key]).join(" ");
    }

    return message;
  }

  // Set generic error logic message
  setUserNameMessage(c: AbstractControl): string {
    let message: string = "";

    if (c.touched || c.dirty) {
      if (c.errors) {
        message = Object.keys(c.errors).map(key =>
          this.validationMessages[key]).join(" ");
      } else if (c.value.toLowerCase() !== this.currentUserName) {
        this.memberService.isUserNameUnique(c.value).subscribe(res => {
          if (res === false) {
            this.userNameMessage = "The user name must be unique";
            c.setErrors({ "notUnique": true });
          }
        }); 
      }
    }

    return message;
  }

  // Set Phone Number error message
  setPhoneNumberMessage(c: AbstractControl): string {
    let message: string = "";

    if ((c.touched || c.dirty) && c.value) {

      // Remove any non-numeric values
      var phoneNumberText = c.value.match(/\d/g);

      // If only non-numeric values are entered, blank out the field
      if (phoneNumberText == null) {
        c.setValue("");
      } else {
        var phoneNumber: string = phoneNumberText.join("").toString();

        if (phoneNumber.length !== 10) {
          message = "Phone number must be 10 digits";
          c.setErrors({ "invalidFormat": true });
        } else {

          // If the number is valid, format it
          c.setValue(this.phonePipe.transform(phoneNumber));
        }
      }
    }

    return message;
  }

  // Set Date Of Birth error message
  setDateOfBirthMessage(c: AbstractControl): string {
    let message: string = "";

    if (c.touched || c.dirty) {
      if (!this.moment(c.value, "M/D/YYYY", true).isValid()) {
        message = "Must be a valid date (MM/DD/YYYY)";
        c.setErrors({ "invalidFormat": true });
      }
    }

    return message;
  }

  saveMember(formValues) {
    if (this.memberForm.valid) {
      this.submitInProgress = true;

      var formMember: IMember = {
        id: formValues.id,
        userName: formValues.userName,
        firstName: formValues.firstName,
        lastName: formValues.lastName,
        email: formValues.email,
        dateOfBirth: formValues.dateOfBirth
      };

      if (formValues.phoneNumber) {
        formMember.phoneNumber = formValues.phoneNumber.match(/\d/g).join("").toString();
      }

      if (this.inputMode === InputModeType.Add) {
        this.memberService.addMember(formMember).subscribe(
          () => {
            this.closeModal();
          },
          error => {
            this.dataError = true;
            this.submitInProgress = false;
            this.dataErrorMessage = "An error occurred adding the Member";
            console.log("Error calling memberService.getMember()");
            console.log(error);
          }
        );
      }

      if (this.inputMode === InputModeType.Edit) {
        this.memberService.updateMember(formMember).subscribe(
          () => {
            this.closeModal();
          },
          error => {
            this.dataError = true;
            this.submitInProgress = false;
            this.dataErrorMessage = "An error occurred saving the Member";
            console.log("Error calling memberService.getMember()");
            console.log(error);
          }
        );
      }
    }
  }

  closeModal() {
    this.activeModal.close();
    this.reloadMemberList.emit();
    this.submitInProgress = false;
  }
}
