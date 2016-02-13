using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using RowanAdams.Entities.Models;
using Service.Pattern;
using RowanAdams.Queries;

namespace RowanAdams.Services
{
	public interface ILogEntryService : IService<LogEntry>
	{
		Task<LogEntry> GetLogEntry(Guid id);
		Task<IEnumerable<LogEntry>> GetAllLogEntries();
		Task<IEnumerable<LogEntry>> GetLogEntriesDuringMonthYear(int month, int year);
	}

	public class LogEntryService : Service<LogEntry>, ILogEntryService
	{
		private readonly IRepositoryAsync<LogEntry> _repository;

		public LogEntryService(IRepositoryAsync<LogEntry> repository) : base(repository)
		{
			_repository = repository;
		}

		public async Task<LogEntry> GetLogEntry(Guid id)
		{
			var logEntry = await this.Query(c => c.Id == id)
				.SelectAsync();

			return logEntry.Single();
		}

		public async Task<IEnumerable<LogEntry>> GetAllLogEntries()
		{
			return await this.Query()
				.Include(x => x.Chore)
				.OrderBy(le => le.OrderByDescending(x => x.CompletedDate))
				.SelectAsync();
		}

		public async Task<IEnumerable<LogEntry>> GetLogEntriesDuringMonthYear(int month, int year)
		{
            // If we wanted data in a different shape we could also do something like this...
            //return await _repository.GetLogEntriesWithChores().ToListAsync();

			var startDate = new DateTime(year, month, 1);
			var endDate = startDate.AddMonths(1);
			return await this.Query(x => x.CompletedDate >= startDate && x.CompletedDate < endDate)
				.Include(x => x.Chore)
				.OrderBy(le => le.OrderByDescending(x => x.CompletedDate))
				.SelectAsync();
		}
	}
}
