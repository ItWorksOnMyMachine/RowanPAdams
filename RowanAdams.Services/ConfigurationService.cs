using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowanAdams.Services
{
	public interface IConfigurationService
	{
		string GetAppSetting(string key);
	}

	public class ConfigurationService : IConfigurationService
	{
		public string GetAppSetting(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
