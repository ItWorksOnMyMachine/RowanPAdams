using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace RowanAdams.Entities.Models
{
	public partial class LogEntry: Entity
	{
		public Guid Id { get; set; }
		public Guid ChoreId { get; set; }
		public DateTime CompletedDate { get; set; }
		public Chore Chore { get; set; }
	}
}
