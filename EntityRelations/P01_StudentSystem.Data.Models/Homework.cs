using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P01_StudentSystem.Data.Models.Enums;

namespace P01_StudentSystem.Data.Models;

public class Homework
{
    [Key]
    public int HomeworkId { get; set; }

    [Column("ContentFilePath",TypeName = "VARCHAR(260)")]
    public string Content { get; set; }

    public ContentTypeEnum ContentType { get; set; }

    public DateTime SubmissionTime { get; set; }

    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; }

    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }
}