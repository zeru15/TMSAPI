using MediatR;
using TmsApi.Application.DTOs;

namespace TmsApi.Application.Courses.Queries;

public record GetCoursesQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    string OrderBy = "Title",
    bool Descending = false)
    : IRequest<PagedResponse<CourseResponseDto>>;