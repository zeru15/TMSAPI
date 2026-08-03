import { Component, effect, input } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
  selector: "app-course-detail",
  standalone: true,
  imports: [RouterLink],
  templateUrl: "./course-detail.component.html",
  styleUrl: "./course-detail.component.scss",
})
export class CourseDetailComponent {
  // Receives the :id value from the URL
  id = input.required<string>();

  constructor() {
    effect(() => {
      console.log(`Loading course detail for ID: ${this.id()}`);
    });
  }
}