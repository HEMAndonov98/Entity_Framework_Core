using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P01_StudentSystem.Data.Models.Enums;

namespace P01_StudentSystem.Data.Models;

public class Resource
{
    [Key]
    public int ResourceId { get; set; }

    [Column("ResourceName", TypeName = "NVARCHAR(50)")]
    public string Name { get; set; }

    [Column("ResourceUrl", TypeName = "VARCHAR(2048)")]
    public string Url { get; set; }

    public ResourceTypeEnum ResourceType { get; set; }

    public int CourseId { get; set; }
   
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }
}