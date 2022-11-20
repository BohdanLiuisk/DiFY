import { DialogRef } from "@angular/cdk/dialog";
import { Component, OnInit } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";

@Component({
    selector: 'app-create-new-call',
    templateUrl: './create-new-call.component.html',
    styleUrls: ['./create-new-call.component.scss']
})
export class CreateNewCallComponent implements OnInit {
  public nameControl: FormControl;

  constructor(protected _dialogRef: DialogRef<{ name: string }>) { }

  public ngOnInit(): void {
    this.nameControl = new FormControl('',
      [Validators.required]
    );
  }

  public save(): void {
    if(this.nameControl.valid) {
      this._dialogRef.close({ name: this.nameControl.value });
    } else {
      this.nameControl.markAsTouched();
    }
  }

  public cancel(): void {
    this._dialogRef.close();
  }
}
