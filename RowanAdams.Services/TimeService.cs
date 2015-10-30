using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowanAdams.Services
{
	public interface ITimeService
	{
		DateTime Now { get; }
		DateTime UtcNow { get; }
	}

	public class TimeService: ITimeService
	{
		public DateTime Now
		{
			get { return DateTime.Now; } 
		}

		public DateTime UtcNow
		{
			get { return DateTime.UtcNow; } 
			
		}
	}
}
