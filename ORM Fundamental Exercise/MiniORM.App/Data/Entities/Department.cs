using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;

namespace MiniORM.App.Data.Entities
{
	[Table("Departments")]
	public class Department
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? Name { get; set; }

		public ICollection<Employee>? Employees { get; }
	}
}

