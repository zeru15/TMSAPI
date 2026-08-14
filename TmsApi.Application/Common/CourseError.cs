namespace TmsApi.Application.Common;

public sealed record CourseError(string Code, string Message)
{
    public static CourseError NotFound(int id) =>
        new(
            "course_not_found",
            $"Course with ID '{id}' was not found.");

    public static CourseError NotFoundByCode(string code) =>
        new(
            "course_not_found",
            $"Course '{code}' was not found.");

    public static CourseError DuplicateCode(string code) =>
        new(
            "duplicate_course_code",
            $"A course with code '{code}' already exists.");
}