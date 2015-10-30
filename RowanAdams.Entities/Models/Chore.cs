using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace RowanAdams.Entities.Models
{
	public partial class Chore : Entity
	{
		public Guid Id { get; set; }
		[Required]
		public string Name { get; set; }
		public decimal Value { get; set; }
		public bool Active { get; set; } = true;
		public DateTime CreatedDate { get; set; }
		public virtual ICollection<LogEntry> LogEntries { get; set; } = new HashSet<LogEntry>();
	}
}
