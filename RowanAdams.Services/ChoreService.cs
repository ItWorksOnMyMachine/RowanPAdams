using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using RowanAdams.Entities.Models;
using Service.Pattern;

namespace RowanAdams.Services
{
	public interface IChoreService : IService<Chore>
	{
		Task<Chore> GetChore(Guid id);
		Task<IEnumerable<Chore>> GetAllChores();
		Task<IEnumerable<Chore>> GetActiveChores();
	}

	public class ChoreService : Service<Chore>, IChoreService
	{
		private readonly IRepositoryAsync<Chore> _repository;

		public ChoreService(IRepositoryAsync<Chore> repository) : base(repository)
        {
			_repository = repository;
		}

		public async Task<Chore> GetChore(Guid id)
		{
			var chore = await this.Query(c => c.Id == id)
				.SelectAsync();

			return chore.Single();
		}

		public async Task<IEnumerable<Chore>> GetActiveChores()
		{
			return await this.Query(c => c.Active)
				.OrderBy(c => c.OrderByDescending(x => x.Active)
				.ThenBy(x => x.Name)
				.ThenByDescending(x => x.CreatedDate))
				.SelectAsync();

		}

		public async Task<IEnumerable<Chore>> GetAllChores()
		{
			return await this.Query()
				.OrderBy(c => c.OrderByDescending(x => x.Active)
					.ThenBy(x => x.Name)
					.ThenByDescending(x => x.CreatedDate))
				.SelectAsync();
		}
	}
}
