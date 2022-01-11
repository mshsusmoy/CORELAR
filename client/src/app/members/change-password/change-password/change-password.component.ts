import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  changePasswordForm : FormGroup;

  constructor(private accountService: AccountService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router) { }

    ngOnInit(): void {
      this.initializeForm();
    }

    initializeForm(){
      this.changePasswordForm = this.fb.group({
        currentPassword: ['',[Validators.required, Validators.minLength(4),Validators.maxLength(8)]],
        newPassword: ['',[Validators.required, Validators.minLength(4),Validators.maxLength(8)]],
        confirmNewPassword: ['',[Validators.required, this.matchValues('newPassword')]]
      })
      this.changePasswordForm.controls.password.valueChanges.subscribe(() =>{
        this.changePasswordForm.controls.confirmPassword.updateValueAndValidity();
      })
    }

    matchValues(matchTo: string): ValidatorFn{
      return (control: AbstractControl) => {
        return control?.value === control?.parent?.controls[matchTo].value 
          ? null : {isMatching: true} 
      }
    }

    changePassword(){
      this.accountService.changePassword(this.changePasswordForm.value).subscribe(response =>{
        this.router.navigateByUrl('/members');
      }, error =>{
        this.toastr.error("Cannot Change Password");
      })
    }

    cancel(){
      this.router.navigateByUrl('/members');
    }

}
