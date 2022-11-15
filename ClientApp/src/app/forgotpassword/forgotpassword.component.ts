import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


export interface DialogData {
  heading: string;
  emailReturned: string;
}


@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent {

  constructor(public dialogRef: MatDialogRef<ForgotpasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {

  }

  onCancelClick(): void {
    this.dialogRef.close();
  }


  // use below function to listen if user closes the dialog (whether by clicking any button or by clicking outside)
  // It will send this.data to the dialogRef decalred in openForgotPasswordDialog() method (in Home Component)
  //ngOnDestroy() {
  //  this.dialogRef.close(this.data);
  //}

}
