var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();

builder.Services.AddSingleton<EnrollmentWorker>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Host.UseDefaultServiceProvider(options =>
{
options.ValidateScopes = true;
options.ValidateOnBuild = true;
});


builder.Services
    .AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();


var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

// Configure the HTTP request pipeline.
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePages();


// app.MapGet("/api/assessments/results", () => Results.Ok(new
// {
// courseCode = "CS-101", 
// studentId = "S-001",
// letterGrade = "A"
// })).RequireAuthorization();

app.MapGet("/api/error", () =>
{
    throw new TmsDatabaseException(
        "Simulated database failure for ProblemDetails testing");
});


app.Run();

