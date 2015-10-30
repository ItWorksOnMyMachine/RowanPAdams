using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;
using RowanAdams.Entities.Models;

namespace RowanAdams.Tests.Fakes.Entities
{
	public class FakeEntitiesContext : FakeDbContext
	{
		public FakeEntitiesContext()
		{
			AddFakeDbSet<Chore, FakeChoreDbSet>();
			AddFakeDbSet<LogEntry, FakeLogEntryDbSet>();
		}
	}
}
