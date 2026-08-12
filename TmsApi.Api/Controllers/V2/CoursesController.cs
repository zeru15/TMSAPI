using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TmsApi.Application.Common;
using TmsApi.Application.Courses.Commands;
using TmsApi.Application.Courses.Queries;

namespace TmsApi.Api.Controllers.V2;

[ApiController]
[Route("api/v{version:apiVersion}/courses")]
[ApiVersion("2.0")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourse(
        int id,
        CancellationToken ct)
    {
        var course = await mediator.Send(
            new GetCourseByIdQuery(id),
            ct);

        return course is not null
            ? Ok(course)
            : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses(
        [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null,
        CancellationToken ct = default)
    {
        // No paging parameters → return all courses
        if (page is null && pageSize is null)
        {
            var courses = await mediator.Send(
                new GetAllCoursesQuery(),
                ct);

            return Ok(new
            {
                data = courses,
                links = new
                {
                    self = "/api/v2/courses",
                    enroll = "/api/v2/enrollments"
                }
            });
        }

        // Paging was requested
        var requestedPage = Math.Max(1, page ?? 1);
        var requestedPageSize = Math.Clamp(pageSize ?? 20, 1, 50);

        var result = await mediator.Send(
            new GetCoursesQuery(
                requestedPage,
                requestedPageSize),
            ct);

        return Ok(new
        {
            data = result.Items,
            meta = new
            {
                result.TotalCount,
                result.Page,
                result.PageSize,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            },
            links = new
            {
                self =
                    $"/api/v2/courses?page={result.Page}&pageSize={result.PageSize}",

                next = result.HasNext
                    ? $"/api/v2/courses?page={result.Page + 1}&pageSize={result.PageSize}"
                    : null,

                prev = result.HasPrevious
                    ? $"/api/v2/courses?page={result.Page - 1}&pageSize={result.PageSize}"
                    : null,

                enroll = "/api/v2/enrollments"
            }
        });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCourse(
        CreateCourseCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(
            onSuccess: created =>
                CreatedAtAction(
                    nameof(GetCourse),
                    new { id = created.Id },
                    created),

            onFailure: error =>
            {
                var status = error.Code switch
                {
                    "course_not_found" =>
                        StatusCodes.Status404NotFound,

                    "duplicate_course_code" =>
                        StatusCodes.Status409Conflict,

                    _ =>
                        StatusCodes.Status400BadRequest
                };

                return Problem(
                    statusCode: status,
                    title: "Course operation rejected",
                    detail: error.Message,
                    type: $"https://tms.local/errors/{error.Code}");
            });
    }
}
















// using Asp.Versioning;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using TmsApi.Infrastructure.Persistence;

// namespace TmsApi.Api.Controllers.V2;

// [ApiController]
// [Route("api/v{version:apiVersion}/courses")]
// [ApiVersion("2.0")]
// public class CoursesController(TmsDbContext context) : ControllerBase
// {
//     [HttpGet]
//     public async Task<IActionResult> GetCourses(
//     [FromQuery] int page = 1,
//     [FromQuery] int pageSize = 20,
//     CancellationToken ct = default)
//     {
//         page = Math.Max(1, page);
//         pageSize = Math.Clamp(pageSize, 1, 50);
//         var baseQuery = context.Courses.AsNoTracking();
//         var totalCount = await baseQuery.CountAsync(ct);

//         var rows = await baseQuery
//         .OrderBy(c => c.Title)
//         .Skip((page - 1) * pageSize)
//         .Take(pageSize)
//         .Select(c => new
//         {
//             c.Id,
//             c.Title,
//             c.Code,
//             c.MaxCapacity,
//             EnrollmentCount = c.Enrollments.Count
//         })
//         .ToListAsync(ct);
//         var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
//         var hasNext = page < totalPages;
//         var hasPrevious = page > 1;
//         return Ok(new
//         {
//             data = rows,
//             meta = new
//             {
//                 totalCount,
//                 page,
//                 pageSize,
//                 totalPages,
//                 hasNext,
//                 hasPrevious
//             },
//             links = new
//             {
//                 self = $"/api/v2/courses?page={page}&pageSize={pageSize}",
//                 next = hasNext
//         ? $"/api/v2/courses?page={page + 1}&pageSize={pageSize}"
//         : (string?)null,
//                 prev = hasPrevious
//         ? $"/api/v2/courses?page={page - 1}&pageSize={pageSize}"
//         : (string?)null,
//                 enroll = "/api/v2/enrollments"
//             }
//         });
//     }
// }