import { Component, inject, signal } from "@angular/core";
import {
  FormBuilder,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";

@Component({
  selector: "app-enrollment-form",
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: "./enrollment-form.component.html",
  styleUrl: "./enrollment-form.component.scss",
})
export class EnrollmentFormComponent {
  // Angular dependency injection:
  // similar to receiving a service through a C# constructor
  private fb = inject(FormBuilder);

  // Controls whether the success message is displayed
  submitted = signal(false);

  // The complete reactive form
  form = this.fb.nonNullable.group({
    studentId: [
      "",
      [
        Validators.required,
        Validators.pattern("^STU-[0-9]{4}$"),
      ],
    ],

    courseId: [
      "",
      Validators.required,
    ],

    term: [
      "Fall 2026",
      Validators.required,
    ],

    notes: [""],

    // Starts with no backup-course inputs
    backupCourses: this.fb.array<FormControl<string>>([]),
  });

  // Shortcut for accessing the FormArray
  get backups() {
    return this.form.controls.backupCourses;
  }

  // Adds one backup-course input
  addBackup() {
    this.backups.push(
      this.fb.control("", {
        nonNullable: true,
        validators: Validators.required,
      }),
    );
  }

  // Removes the backup input at the specified position
  removeBackup(index: number) {
    this.backups.removeAt(index);
  }

  submit() {
    if (this.form.valid) {
      // Gets all form values, including disabled controls
      const payload = this.form.getRawValue();

      console.log("Enrollment payload:", payload);

      this.submitted.set(true);
    } else {
      // Makes validation messages appear for all invalid fields
      this.form.markAllAsTouched();
    }
  }
}