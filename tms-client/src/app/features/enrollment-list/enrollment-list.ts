// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-enrollment-list',
//   imports: [],
//   templateUrl: './enrollment-list.html',
//   styleUrl: './enrollment-list.scss',
// })
// export class EnrollmentList {}

import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import { EnrollmentStore } from '../../store/enrollment.store';

@Component({
  selector: 'app-enrollment-list',
  standalone: true,
  imports: [],
  templateUrl: './enrollment-list.html',
  styleUrl: './enrollment-list.scss',
})
export class EnrollmentList implements OnInit {

  readonly store = inject(EnrollmentStore);

  ngOnInit(): void {
    this.store.loadEnrollments();
  }

  approve(id: string): void {
    this.store.approveEnrollment(id);
  }
}