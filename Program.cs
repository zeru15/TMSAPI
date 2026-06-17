var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/api/assessments/results", () => Results.Ok(new
{
courseCode = "CS-101", 
studentId = "S-001",
letterGrade = "A"
})).RequireAuthorization();


app.Run();

