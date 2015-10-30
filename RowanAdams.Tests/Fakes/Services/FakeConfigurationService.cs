using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowanAdams.Services;

namespace RowanAdams.Tests.Fakes.Services
{
	public class FakeConfigurationService: IConfigurationService
	{
		public IDictionary<string, string> _appSettings = new Dictionary<string, string>();

		public void SetAppSetting(string key, string value)
		{
			_appSettings.Add(key, value);
		}

		public string GetAppSetting(string key)
		{
			string value;
			if (_appSettings.TryGetValue(key, out value))
				return value;
			else
				return null;
		}
	}
}
