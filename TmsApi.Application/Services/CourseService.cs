using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TmsApi.Application.DTOs;
using TmsApi.Domain.Entities;
using TmsApi.Infrastructure.Persistence;

namespace TmsApi.Application.Services;

public class CourseService(TmsDbContext context, ILogger<CourseService> logger) : ICourseService
{
    // public async Task<Course?> GetByIdAsync(int id, CancellationToken ct)
    // {
    //     return await context.Courses
    //         .AsNoTracking()
    //         .FirstOrDefaultAsync(c => c.Id == id, ct);
    // }
    public Task<CourseResponseDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return context.Courses
                      .AsNoTracking()
                      .Where(c => c.Id == id)
                      .Select(c => new CourseResponseDto(
                      c.Id, c.Code, c.Title, c.MaxCapacity, c.Enrollments.Count))
                      .FirstOrDefaultAsync(ct);

    }

    // public async Task<Course> CreateAsync(Course course, CancellationToken ct)
    // {
    //     context.Courses.Add(course);
    //     await context.SaveChangesAsync(ct);
    //     logger.LogInformation("Created new course with ID {CourseId} ({CourseCode})",
    //     course.Id, course.Code);
    //     return course;
    // }
    public async Task<CourseResponseDto> CreateAsync(CreateCourseRequest request, CancellationToken ct)
    {
        var course = new Course
        {
            Code = request.Code,
            Title = request.Title,
            MaxCapacity = request.MaxCapacity
        };
        context.Courses.Add(course);

        await context.SaveChangesAsync(ct);

        logger.LogInformation("Created course {CourseId} ({Code})", course.Id, course.Code);

        return (await GetByIdAsync(course.Id, ct))!;
    }

    public Task<bool> CodeExistsAsync(string code, CancellationToken ct)
    {
        return context.Courses
            .AsNoTracking()
            .AnyAsync(c => c.Code == code, ct);
    }

    public async Task<PagedResponse<CourseResponseDto>> GetCoursesAsync(
    PagedRequest request,
    CancellationToken ct)
    {
        // Step 1: Start with a no-tracking query
        IQueryable<Course> query = context.Courses.AsNoTracking();

        // Step 2: Apply search filter (case-insensitive)
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(c =>
                EF.Functions.ILike(c.Title, $"%{request.Search}%") ||
                EF.Functions.ILike(c.Code, $"%{request.Search}%"));
        }

        // Step 3: Count BEFORE paging
        var totalCount = await query.CountAsync(ct);

        // Step 4: Apply ordering
        query = request.OrderBy switch
        {
            "Code" => request.Descending
                ? query.OrderByDescending(c => c.Code)
                : query.OrderBy(c => c.Code),

            "MaxCapacity" => request.Descending
                ? query.OrderByDescending(c => c.MaxCapacity)
                : query.OrderBy(c => c.MaxCapacity),

            _ => request.Descending
                ? query.OrderByDescending(c => c.Title)
                : query.OrderBy(c => c.Title)
        };

        // Step 5: Paging + projection
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CourseResponseDto(
                c.Id,
                c.Code,
                c.Title,
                c.MaxCapacity,
                c.Enrollments.Count))
            .ToListAsync(ct);

        // Step 6: Return paged response
        return new PagedResponse<CourseResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}