using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowanAdams.Entities.Models.Mapping
{
	public class LogEntryMap : EntityTypeConfiguration<LogEntry>
	{
		public LogEntryMap()
		{
			ToTable("LogEntries");

			HasKey(t => t.Id);

			Property(t => t.Id)
				.IsRequired()
				.HasColumnName("Id");

			Property(t => t.ChoreId)
				.IsRequired()
				.HasColumnName("ChoreId");

			Property(t => t.CompletedDate)
				.IsRequired()
				.HasColumnName("CompletedDate")
				.HasColumnType("date");

			HasRequired(t => t.Chore)
				.WithMany(t => t.LogEntries)
				.HasForeignKey(t => t.ChoreId);
		}
	}
}
