using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    [Column("StudentName", TypeName = "NVARCHAR(100)")]
    public string Name { get; set; }
    
    [Column("StudentPhoneNumber", TypeName = "VARCHAR(10)")]
    public string? PhoneNumber { get; set; }

    [Column("DateRegisteredOn", TypeName = "DATETIME2")]
    public DateTime RegisteredOn { get; set; }

    [Column("StudentBirthday", TypeName = "DATETIME2")]
    public DateTime? Birthday { get; set; }

    [InverseProperty(nameof(Student))]
    public ICollection<Homework> Homeworks { get; set; }

    [InverseProperty(nameof(Student))]
    public ICollection<StudentCourse> StudentsCourses { get; set; }
}