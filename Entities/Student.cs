namespace TmsApi.Entities;

public class Student
{
    // surrogate primary key — internal, used by foreign keys
    public int Id {get; set;}
    // natural key — human-readable (uniqueness configured in Session 2)
    public required string RegistrationNumber {get; set;}
    public required string Name {get; set;}
    public decimal GPA {get; set;}
    public bool IsActive {get; set;} = true;
    // Navigation property for many-to-many relationship
    public ICollection<Enrollment> Enrollments {get; set;} = new List<Enrollment>();
}