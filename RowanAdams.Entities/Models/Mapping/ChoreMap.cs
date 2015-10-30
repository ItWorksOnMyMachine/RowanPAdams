using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowanAdams.Entities.Models.Mapping
{
	public class ChoreMap : EntityTypeConfiguration<Chore>
	{
		public ChoreMap()
		{
			ToTable("Chores");

			HasKey(t => t.Id);

			Property(t => t.Id)
				.IsRequired()
				.HasColumnName("Id");

			Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(50)
				.HasColumnName("Name");

			Property(t => t.Value)
				.IsRequired()
				.HasPrecision(19, 4)
				.HasColumnName("Value");

			Property(t => t.Active)
				.IsRequired()
				.HasColumnName("Active");

			Property(t => t.CreatedDate)
				.IsRequired()
				.HasColumnName("CreatedDate")
				.HasColumnType("date");
		}
	}
}
