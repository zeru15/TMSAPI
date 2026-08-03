import { Component, computed, signal } from "@angular/core";
import { CourseCardComponent } from "../../ui/course-card/course-card.component";
import { Course } from "../../models/course.model";

@Component({
  selector: "app-student-dashboard",
  standalone: true,
  imports: [CourseCardComponent],
  templateUrl: "./student-dashboard.component.html",
  styleUrl: "./student-dashboard.component.scss",
})
export class StudentDashboardComponent {
  studentName = signal("Liya Kebede");

  earnedCredits = signal(45);

  graduationStatus = computed(() =>
    this.earnedCredits() >= 120
      ? "Eligible for Graduation"
      : "In Progress",
  );

  selectedCourse = signal<Course | null>(null);

  sampleCourse: Course = {
    id: 1,
    title: "Advanced Java Services",
    code: "CSE-101",
    maxCapacity: 30,
    enrollmentCount: 12,
  };

  registerForClass() {
    this.earnedCredits.update((credits) => credits + 3);
  }

  handleEnroll(course: Course) {
    this.selectedCourse.set(course);

    console.log("Enrollment requested for:", course.title);
  }
}