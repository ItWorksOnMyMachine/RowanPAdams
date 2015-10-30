using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using RowanAdams.ActionResults;
using RowanAdams.Entities.Models;
using RowanAdams.Services;

namespace RowanAdams.Controllers
{
	public class LogEntryBO
	{
		public Guid Id { get; set; }
		public Guid ChoreId { get; set; }
		public string Name { get; set; }
		public decimal Value { get; set; }
		public DateTime CompletedDate { get; set; }
	}

	public class LogEntryGroupingBO
	{
		public string GroupName { get; set; }
		public decimal GroupTotal { get; set; }
		public IEnumerable<LogEntryBO> LogEntries { get; set; }
	}

	[Authorize]
	public class LogEntriesController : ApiController
    {
		private readonly ILogEntryService _logEntryService;
		private readonly IUnitOfWorkAsync _unitOfWorkAsync;

		public LogEntriesController(IUnitOfWorkAsync unitOfWorkAsync, ILogEntryService logEntryService)
		{
			_unitOfWorkAsync = unitOfWorkAsync;
			_logEntryService = logEntryService;
		}

		[HttpGet]
		[Route("api/logentries/{id}", Name = "GetLogEntry")]
		public async Task<LogEntryBO> GetLogEntry(Guid id)
		{
			var logEntry = await _logEntryService.GetLogEntry(id);

			return new LogEntryBO() {
				Id = logEntry.Id,
				ChoreId = logEntry.ChoreId,
				Name = logEntry.Chore.Name,
				Value = logEntry.Chore.Value,
				CompletedDate = logEntry.CompletedDate
			};
		}

		[HttpGet]
		[Route("api/logentries/month")]
		public async Task<IEnumerable<LogEntryBO>> GetByMonth(DateTime monthStartingOn)
		{
			var logEntries = await _logEntryService.GetLogEntriesDuringMonthYear(monthStartingOn.Month, monthStartingOn.Year);
			return logEntries.Select(x => new LogEntryBO() {
				Id = x.Id,
				ChoreId = x.ChoreId,
				Name = x.Chore.Name,
				Value = x.Chore.Value,
				CompletedDate = x.CompletedDate
			});
		}

		[HttpGet]
		[Route("api/logentries/all")]
		public async Task<IEnumerable<LogEntryGroupingBO>> GetAllGroupedByMonth()
		{
			var logEntries = await _logEntryService.GetAllLogEntries();

			return logEntries
				.OrderByDescending(le => le.CompletedDate)
				.GroupBy(e => e.CompletedDate.ToString("MMMM yyyy"))
				.Select(g => new LogEntryGroupingBO() {
					GroupName = g.Key,
					GroupTotal = g.Sum(le => le.Chore.Value),
					LogEntries = g.Select(x => new LogEntryBO() {
						Id = x.Id,
						ChoreId = x.ChoreId,
						Name = x.Chore.Name,
						Value = x.Chore.Value,
						CompletedDate = x.CompletedDate
					}).OrderByDescending(le => le.CompletedDate)
				});
		}

		[HttpPost]
		[Route("api/logentries")]
		public async Task<IHttpActionResult> Post(LogEntry logEntry)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			logEntry.Id = Guid.NewGuid();
			logEntry.ObjectState = ObjectState.Added;
			_logEntryService.Insert(logEntry);

			await _unitOfWorkAsync.SaveChangesAsync();

			return new CreatedContentActionResult(Request, Url.Link("GetLogEntry", new { id = logEntry.Id }));
		}
	}
}
