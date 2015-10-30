using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using RowanAdams;
using RowanAdams.Controllers;
using RowanAdams.Entities;
using RowanAdams.Entities.Models;
using RowanAdams.Services;
using RowanAdams.Tests.Fakes.Entities;
using RowanAdams.Tests.Fakes.Services;

namespace RowanAdams.Tests.Controllers
{
	[TestClass]
	public class ChoresControllerTests
	{
		[TestMethod]
		public async Task EnsurePostCreatesNewChoreAndSetsHttpStatus()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)))
					.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");

				var result = await controller.Post(new Chore { Name = "Test Chore 1", Value = 10 });
				var response = await result.ExecuteAsync(new CancellationToken());

				Assert.AreEqual(1, context.Chores.Count());
				var chore = context.Chores.Single();
                Assert.AreNotEqual(Guid.Empty, chore.Id);
				Assert.AreEqual(new DateTime(2015, 11, 1), chore.CreatedDate);
				Assert.IsTrue(chore.Active);
				Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
				Assert.AreEqual($"/api/chores/{chore.Id}", response.Headers.Location.AbsolutePath);
			}
		}

		[TestMethod]
		public async Task EnsurePostReturnsErrorWhenInvalidModel()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)));

				controller.ModelState.AddModelError("TestError", new Exception());
				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");
				var result = await controller.Post(new Chore { });
				var response = await result.ExecuteAsync(new CancellationToken());
				Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
			}
		}

		[TestMethod]
		public async Task EnsurePutUpdatesExistingChoreAndSetsHttpStatus()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)));
					
				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");
				var result = await controller.Post(new Chore { Name = "Test Chore 1", Value = 10 });
				var chore = context.Chores.Single();
				chore.Active = false;

				controller.SetControllerContext(HttpMethod.Put, "http://localhost/RowanAdams/api/chores");
				result = await controller.Put(chore);
				var response = await result.ExecuteAsync(new CancellationToken());

				var chore2 = context.Chores.Single();
				Assert.AreEqual(chore.Id, chore2.Id);
				Assert.IsFalse(chore.Active);
				Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
				Assert.AreEqual($"/api/chores/{chore2.Id}", response.Headers.Location.AbsolutePath);
			}
		}

		[TestMethod]
		public async Task EnsurePutReturnsNotFoundWhenChoreDoesNotExist()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)));

				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");
				var result = await controller.Post(new Chore { Name = "Test Chore 1", Value = 10 });

				controller.SetControllerContext(HttpMethod.Put, "http://localhost/RowanAdams/api/chores");
				result = await controller.Put(new Chore() {Id = Guid.NewGuid(), Name = "Test Chore 1" });
				var response = await result.ExecuteAsync(new CancellationToken());
				Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
			}
		}

		[TestMethod]
		public async Task EnsurePutReturnsErrorWhenInvalidModel()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)));

				controller.ModelState.AddModelError("TestError", new Exception());
				controller.SetControllerContext(HttpMethod.Put, "http://localhost/RowanAdams/api/chores");
				var result = await controller.Put(new Chore { });
				var response = await result.ExecuteAsync(new CancellationToken());
				Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
			}
		}

		[TestMethod]
		public async Task GetAllGetsActiveAndInactiveChores()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)));
					
				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");
				await controller.Post(new Chore { Name = "Test Chore 1", Value = 1 });
				await controller.Post(new Chore { Name = "Test Chore 2", Value = 2 });
				await controller.Post(new Chore { Name = "Test Chore 3", Value = 3 });
				await controller.Post(new Chore { Name = "Test Chore 4", Value = 4 });

				var chore = context.Chores.First();
				chore.Active = false;

				controller.SetControllerContext(HttpMethod.Put, "http://localhost/RowanAdams/api/chores");
				await controller.Put(chore);

				controller.SetControllerContext(HttpMethod.Get, "http://localhost/RowanAdams/api/chores/all");
				var chores = await controller.GetAll();

				Assert.AreEqual(4, chores.Count());
			}
		}

		[TestMethod]
		public async Task GetAllActiveGetsOnlyActiveChores()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)));

				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");
				await controller.Post(new Chore { Name = "Test Chore 1", Value = 1 });
				await controller.Post(new Chore { Name = "Test Chore 2", Value = 2 });
				await controller.Post(new Chore { Name = "Test Chore 3", Value = 3 });
				await controller.Post(new Chore { Name = "Test Chore 4", Value = 4 });

				var chore = context.Chores.First();
				chore.Active = false;

				controller.SetControllerContext(HttpMethod.Put, "http://localhost/RowanAdams/api/chores");
				await controller.Put(chore);

				controller.SetControllerContext(HttpMethod.Get, "http://localhost/RowanAdams/api/chores/active");
				var chores = await controller.GetActive();

				Assert.AreEqual(3, chores.Count());
			}
		}

		[TestMethod]
		public async Task GetChoreGetsChore()
		{
			using (var dbInfo = Utility.CreateSeededTestDatabase())
			using (var context = new EntitiesContext(dbInfo.ConnectionString))
			using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
			{
				IChoreService service = new ChoreService(unitOfWork.RepositoryAsync<Chore>());
				ChoresController controller = new ChoresController(unitOfWork, service, new FakeTimeService(new DateTime(2015, 11, 1)));

				controller.SetControllerContext(HttpMethod.Post, "http://localhost/RowanAdams/api/chores");
				await controller.Post(new Chore { Name = "Test Chore 1", Value = 1 });
				await controller.Post(new Chore { Name = "Test Chore 2", Value = 2 });

				var chore = context.Chores.First();

				controller.SetControllerContext(HttpMethod.Get, $"http://localhost/RowanAdams/api/cores/{chore.Id}");
				var chore2 = await controller.GetChore(chore.Id);

				Assert.AreEqual(chore.Id, chore2.Id);
			}
		}

	}
}
