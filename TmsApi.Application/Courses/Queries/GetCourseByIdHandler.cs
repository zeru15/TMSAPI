using MediatR;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;

namespace TmsApi.Application.Courses.Queries;

public class GetCourseByIdHandler(
    ICourseService courseService)
    : IRequestHandler<GetCourseByIdQuery, CourseResponseDto?>
{
    public async Task<CourseResponseDto?> Handle(
        GetCourseByIdQuery query,
        CancellationToken ct)
    {
        return await courseService.GetByIdAsync(
            query.Id,
            ct);
    }
}