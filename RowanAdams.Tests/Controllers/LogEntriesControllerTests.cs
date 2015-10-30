using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Pattern.Ef6;
using Repository.Pattern.UnitOfWork;
using RowanAdams.Controllers;
using RowanAdams.Entities;
using RowanAdams.Entities.Models;
using RowanAdams.Services;
using RowanAdams.Tests.Fakes.Services;

namespace RowanAdams.Tests.Controllers
{
	[TestClass]
	public class LogEntriesControllerTests
	{
		private DatabaseInformation dbInfo = null;

		[TestInitialize]
		public void Initialize()
		{
			dbInfo = Utility.CreateSeededTestDatabase();

			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)))
					.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");

				var result = controller.Post(new Chore { Name = "Test Chore 1", Value = 10 });
				result.Wait();
			}
		}

		[TestCleanup]
		public void Cleanup()
		{
			dbInfo.Dispose();
		}

		[TestMethod]
		public async Task EnsurePostCreatesNewLogEntryAndSetsHttpStatus()
		{
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				var chore = context.Chores.Single();
				ILogEntryService service = new LogEntryService(unitOfWork.RepositoryAsync<LogEntry>());
				var controller = new LogEntriesController(unitOfWork, service);
					
				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/logentries");
				var result = await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 1)});
				var response = await result.ExecuteAsync(new CancellationToken());

				Assert.AreEqual(1, context.LogEntries.Count());
				var logEntry = context.LogEntries.Single();
				Assert.AreNotEqual(Guid.Empty, logEntry.Id);
				Assert.AreEqual(chore.Id, logEntry.ChoreId);
				Assert.AreEqual(new DateTime(2015, 1, 1), logEntry.CompletedDate);
				Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
				Assert.AreEqual($"/api/logentries/{logEntry.Id}", response.Headers.Location.AbsolutePath);
			}
		}

		[TestMethod]
		public async Task EnsurePostReturnsErrorWhenInvalidModel()
		{
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				var chore = context.Chores.Single();
				ILogEntryService service = new LogEntryService(unitOfWork.RepositoryAsync<LogEntry>());
				var controller = new LogEntriesController(unitOfWork, service);
				controller.ModelState.AddModelError("TestError", new Exception());
				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/logentries");
				var result = await controller.Post(new LogEntry { });
				var response = await result.ExecuteAsync(new CancellationToken());
				Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
			}
		}

		[TestMethod]
		public async Task EnsureGetLogEntryGetsLogEntry()
		{
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				var chore = context.Chores.Single();
				ILogEntryService service = new LogEntryService(unitOfWork.RepositoryAsync<LogEntry>());
				var controller = new LogEntriesController(unitOfWork, service);

				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/logentries");
				var result = await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 1) });
				var logEntry = context.LogEntries.Single();

				controller.SetControllerContext(HttpMethod.Get, $"http://localhost/RowanAdams/api/logentries/{logEntry.Id}");
				var logEntry2 = await controller.GetLogEntry(logEntry.Id);

				Assert.AreEqual(logEntry.Id, logEntry2.Id);
			}
		}

		[TestMethod]
		public async Task EnsureGetByMonthOnlyGetsRequestedMonthEntries()
		{
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				var chore = context.Chores.Single();
				ILogEntryService service = new LogEntryService(unitOfWork.RepositoryAsync<LogEntry>());
				var controller = new LogEntriesController(unitOfWork, service);

				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/logentries");
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 1) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 2) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 3) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 2, 3) });

				controller.SetControllerContext(HttpMethod.Get, $"http://localhost/RowanAdams/api/logentries/month");
				var logEntries = await controller.GetByMonth(new DateTime(2015, 2, 1));

				Assert.AreEqual(1, logEntries.Count());
				var logEntry = logEntries.First();
				Assert.AreEqual(chore.Id, logEntry.ChoreId);
				Assert.AreEqual(chore.Name, logEntry.Name);
				Assert.AreEqual(chore.Value, logEntry.Value);
				Assert.AreEqual(new DateTime(2015, 2, 3), logEntry.CompletedDate);
			}
		}

		[TestMethod]
		public async Task EnsureGetAllGetsAllGroupedByMonthDescending()
		{
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				var chore = context.Chores.Single();
				ILogEntryService service = new LogEntryService(unitOfWork.RepositoryAsync<LogEntry>());
				var controller = new LogEntriesController(unitOfWork, service);

				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/logentries");
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 1) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 2) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 1, 3) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 2, 1) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 2, 2) });
				await controller.Post(new LogEntry { ChoreId = chore.Id, CompletedDate = new DateTime(2015, 2, 3) });

				controller.SetControllerContext(HttpMethod.Get, $"http://localhost/RowanAdams/api/logentries/all");
				var logEntries = await controller.GetAllGroupedByMonth();

				Assert.AreEqual(2, logEntries.Count());
				var logEntryGroup = logEntries.First();
				Assert.AreEqual("February 2015", logEntryGroup.GroupName);
				Assert.AreEqual(30, logEntryGroup.GroupTotal);
				var logEntry = logEntryGroup.LogEntries.First();
				Assert.AreEqual(chore.Id, logEntry.ChoreId);
				Assert.AreEqual(chore.Name, logEntry.Name);
				Assert.AreEqual(chore.Value, logEntry.Value);
				Assert.AreEqual(new DateTime(2015, 2, 3), logEntry.CompletedDate);
				logEntry = logEntryGroup.LogEntries.Skip(1).Take(1).First();
				Assert.AreEqual(new DateTime(2015, 2, 2), logEntry.CompletedDate);
				logEntry = logEntryGroup.LogEntries.Skip(2).Take(1).First();
				Assert.AreEqual(new DateTime(2015, 2, 1), logEntry.CompletedDate);
			}
		}

	}
}
