using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [Column("CourseName", TypeName = "NVARCHAR(80)")]
    public string Name { get; set; }

    [Column("CourseDescritpion", TypeName = "NVARCHAR(300)")]
    public string? Description { get; set; }

    [Column("CourseStartDate", TypeName = "DATETIME2")]
    public DateTime StartDate { get; set; }

    [Column("CourseEndDate", TypeName = "DATETIME2")]
    public DateTime EndDate { get; set; }

    [Column("CoursePrice", TypeName = "DECIMAL(8,2)")]
    public decimal Price { get; set; }

    [InverseProperty(nameof(Course))]
    public ICollection<Resource> Resources { get; set; }

    [InverseProperty(nameof(Course))]
    public ICollection<StudentCourse> StudentsCourses { get; set; }

    [InverseProperty(nameof(Course))]
    public ICollection<Homework> Homeworks { get; set; }
}