using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.dtos;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly TmsDbContext _context;

    public EnrollmentsController(TmsDbContext context)
    {
        _context = context;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetAll()
    // {
    //     var enrollments = await enrollmentService.GetAllAsync();
    //     return Ok(enrollments);
    // }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetById(string id)
    // {
    //     var record = await enrollmentService.GetByIdAsync(id);
    //     return record is not null ? Ok(record) : NotFound();
    // }

    // [HttpPost]
    // public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest request)
    // {
    //     var record = await enrollmentService.EnrollAsync(request.StudentId, request.CourseCode);
    //     return CreatedAtAction(nameof(GetById), new {id = record.Id}, record);
    // }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(string id)
    // {
    //     var deleted = await enrollmentService.DeleteAsync(id);
    //     return deleted ? NoContent() : NotFound();
    // }

    [HttpGet("summary/top-courses")]
    public async Task<ActionResult<IEnumerable<CourseEnrollmentSummaryDto>>> GetTopCourses(
    CancellationToken cancellationToken)
    {
        var result = await _context.Enrollments
            .AsNoTracking()
            .GroupBy(e => new
            {
                e.CourseId,
                e.Course.Title
            })
            .Select(g => new
            {
                g.Key.CourseId,
                g.Key.Title,
                EnrollmentCount = g.Count()
            })
            .OrderByDescending(x => x.EnrollmentCount)
            .Take(5)
            .ToListAsync(cancellationToken);

        var dto = result.Select(x => new CourseEnrollmentSummaryDto(
            x.CourseId,
            x.Title,
            x.EnrollmentCount));

        return Ok(dto);
    }
}

public record CreateEnrollmentRequest(string StudentId, string CourseCode);