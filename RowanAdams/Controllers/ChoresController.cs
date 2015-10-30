using Repository.Pattern.UnitOfWork;
using RowanAdams.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using RowanAdams.Entities.Models;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using RowanAdams.ActionResults;

namespace RowanAdams.Controllers
{
	[Authorize]
    public class ChoresController : ApiController
    {
		private readonly IChoreService _choreService;
		private readonly IUnitOfWorkAsync _unitOfWorkAsync;
		private readonly ITimeService _time;

		public ChoresController(IUnitOfWorkAsync unitOfWorkAsync, IChoreService choreService, ITimeService time)
		{
			_unitOfWorkAsync = unitOfWorkAsync;
			_choreService = choreService;
			_time = time;
		}

		// GET api/chores
		[HttpGet]
		[Route("api/chores/all")]
		public async Task<IEnumerable<Chore>> GetAll()
		{
			return await _choreService.GetAllChores();
		}

		[HttpGet]
		[Route("api/chores/active")]
		public async Task<IEnumerable<Chore>> GetActive()
		{
			return await _choreService.GetActiveChores();
		}

		[HttpGet]
		[Route("api/chores/{id}", Name = "GetChore")]
		public async Task<Chore> GetChore(Guid id)
		{
			return await _choreService.GetChore(id);
		}

		[HttpPut]
		[Route("api/chores")]
		public async Task<IHttpActionResult> Put(Chore chore)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			chore.ObjectState = ObjectState.Modified;
			_choreService.Update(chore);

			try
			{
				await _unitOfWorkAsync.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ChoreExists(chore.Id))
					return NotFound();
				throw;
			}

			return new NoContentActionResult(Request, Url.Link("GetChore", new {id = chore.Id}));
		}

		[HttpPost]
		[Route("api/chores")]
		public async Task<IHttpActionResult> Post(Chore chore)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			chore.Id = Guid.NewGuid();
			chore.Active = true;
			chore.CreatedDate = _time.UtcNow;
			chore.ObjectState = ObjectState.Added;
			_choreService.Insert(chore);

			await _unitOfWorkAsync.SaveChangesAsync();

			return new CreatedContentActionResult(Request, Url.Link("GetChore", new { id = chore.Id }));
		}

		private bool ChoreExists(Guid id)
		{
			return _choreService.Query(e => e.Id == id).Select().Any();
		}
	}
}
