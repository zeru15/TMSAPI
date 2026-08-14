using MediatR;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;

namespace TmsApi.Application.Courses.Queries;

public class SearchCoursesHandler(
    ICourseService courseService)
    : IRequestHandler<SearchCoursesQuery, IReadOnlyList<CourseResponseDto>>
{
    public async Task<IReadOnlyList<CourseResponseDto>> Handle(
        SearchCoursesQuery query,
        CancellationToken ct)
    {
        var result = await courseService.GetCoursesAsync(
            new PagedRequest
            {
                Page = 1,
                PageSize = 50,
                Search = query.Term,
                OrderBy = "Title",
                Descending = false
            },
            ct);

        return result.Items;
    }
}