using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowanAdams.Services;

namespace RowanAdams.Tests.Fakes.Services
{
	public class FakeTimeService : ITimeService
	{
		protected FakeTimeService() {}

		public FakeTimeService(DateTime currentTimeUtc)
		{
			UtcNow = currentTimeUtc;
			Now = currentTimeUtc.ToLocalTime();
		}

		public DateTime Now { get; protected set; }
		public DateTime UtcNow { get; protected set; }
	}
}
