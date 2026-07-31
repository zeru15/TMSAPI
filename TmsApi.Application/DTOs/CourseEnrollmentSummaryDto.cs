namespace TmsApi.Application.DTOs;

public record CourseEnrollmentSummaryDto(
    int CourseId,
    string CourseTitle,
    int EnrollmentCount
);
