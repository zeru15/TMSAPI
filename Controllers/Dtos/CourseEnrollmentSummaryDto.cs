namespace TmsApi.dtos;

public record CourseEnrollmentSummaryDto(
    int CourseId,
    string CourseTitle,
    int EnrollmentCount
);
