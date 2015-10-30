using System.Data.Entity;
using Repository.Pattern.Ef6;
using RowanAdams.Entities.Models;
using RowanAdams.Entities.Models.Mapping;

namespace RowanAdams.Entities
{
	public partial class EntitiesContext : DataContext
	{
		static EntitiesContext()
		{
			Database.SetInitializer<EntitiesContext>(null);
		}

		public EntitiesContext()
			: base("Name=DefaultConnection")
		{
		}

		public EntitiesContext(string connectionString)
			: base(connectionString)
		{
		}

		public DbSet<Chore> Chores { get; set; }
		public DbSet<LogEntry> LogEntries { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new ChoreMap());
			modelBuilder.Configurations.Add(new LogEntryMap());
		}
	}
}
