using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly TmsDbContext _context;

    public StudentsController(TmsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents(
    int page = 1,
    CancellationToken cancellationToken = default)
    {
        const int pageSize = 20;

        var students = await _context.Students
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Ok(students);
    }
}