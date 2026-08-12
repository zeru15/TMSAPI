using MediatR;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;

namespace TmsApi.Application.Courses.Queries;

public class GetCoursesHandler(
    ICourseService courseService)
    : IRequestHandler<GetCoursesQuery, PagedResponse<CourseResponseDto>>
{
    public async Task<PagedResponse<CourseResponseDto>> Handle(
        GetCoursesQuery query,
        CancellationToken ct)
    {
        var request = new PagedRequest
        {
            Page = Math.Max(1, query.Page),
            PageSize = Math.Clamp(query.PageSize, 1, 50),
            Search = query.Search,
            OrderBy = query.OrderBy,
            Descending = query.Descending
        };

        return await courseService.GetCoursesAsync(
            request,
            ct);
    }
}