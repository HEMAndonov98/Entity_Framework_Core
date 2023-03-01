using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("Projects")]
    public class Project
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public ICollection<EmployeeProjects> EmployeeProjects { get; }

		public Project()
		{
		}
	}
}

