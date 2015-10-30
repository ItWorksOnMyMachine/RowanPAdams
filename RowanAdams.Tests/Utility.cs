using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace RowanAdams.Tests
{
	public class DatabaseInformation : IDisposable
	{
		private bool _disposed = false;
		private Server _server;
		private Database _database;

		public string ConnectionString { get; private set; }

		private DatabaseInformation() {}

		public DatabaseInformation(Server server, Database database, string connectionString)
		{
			_server = server;
			_database = database;
			ConnectionString = connectionString;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					// dispose-only, i.e. non-finalizable logic
				}

				try
				{
					_server.KillDatabase(_database.Name);
				}
				catch (Exception)
				{
				}

				_disposed = true;
			}
		}

		~DatabaseInformation()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	public static class Utility
	{
		private static string _sLocalDbInstanceName = null;

        public static DatabaseInformation CreateSeededTestDatabase()
        {
			var uid = Guid.NewGuid().ToString();
			var initialCatalog = $"RPA-{uid}";
	        var localDbInstance = GetLocalDbInstance();
			string tempPath = Path.GetTempPath();//"C:\\Temp\\DCDev-UnitTest";
			if (!Directory.Exists(tempPath))
				Directory.CreateDirectory(tempPath);
			var path = Path.Combine(tempPath, initialCatalog);
	        var mdf = path += ".mdf";
			var ldf = path += ".ldf";

			var server = new Server(localDbInstance);
	        try
	        {
				var db = new Database(server, initialCatalog);
				db.FileGroups.Add(new FileGroup(db, "PRIMARY"));
				var df = new DataFile(db.FileGroups["PRIMARY"], initialCatalog, mdf);
				db.FileGroups["PRIMARY"].Files.Add(df);

				// Add the log file.
				var lf = new LogFile(db, initialCatalog + "_log", ldf);
				db.LogFiles.Add(lf);

				db.Create();

				db.ExecuteNonQuery(Properties.Resources.rpa);

				SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
				{
					DataSource = GetLocalDbInstance(),
					InitialCatalog = initialCatalog,
					PersistSecurityInfo = true,
					IntegratedSecurity = true,
					MultipleActiveResultSets = true
				};

		        return new DatabaseInformation(server, db, sqlBuilder.ToString());
	        }
	        catch (Exception ex)
	        {

		        throw ex;
	        }
        }

		private static string GetConnectionString(string initialCatalog)
		{
			SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
			{
				DataSource = GetLocalDbInstance(),
				AttachDBFilename = GetAttachDbFileNamePath(initialCatalog),
				InitialCatalog = initialCatalog,
				PersistSecurityInfo = true,
				IntegratedSecurity = true,
				MultipleActiveResultSets = true
			};
			return sqlBuilder.ToString();
		}

		private static string GetAttachDbFileNamePath(string fileName)
		{

			string tempPath = Path.GetTempPath();//"C:\\Temp\\DCDev-UnitTest";
			if (!Directory.Exists(tempPath))
				Directory.CreateDirectory(tempPath);
			return Path.Combine(tempPath, fileName);
		}

		private static string GetLocalDbInstance()
		{
			if (_sLocalDbInstanceName == null)
			{
				var p = new Process()
				{
					StartInfo = new ProcessStartInfo("sqllocaldb", " info")
					{
						UseShellExecute = false,
						RedirectStandardOutput = true,
						RedirectStandardError = true
					}
				};

				p.Start();
				p.WaitForExit();

				using (var reader = p.StandardOutput)
				{
					var output = reader.ReadToEnd();
					if (output.IndexOf("v11.0", StringComparison.CurrentCultureIgnoreCase) > -1)
						_sLocalDbInstanceName = "(LocalDb)\\v11.0";
					else if (output.IndexOf("MSSQLLocalDb", StringComparison.CurrentCultureIgnoreCase) > -1)
						_sLocalDbInstanceName = "(LocalDb)\\MSSQLLocalDb";
					else
						throw new Exception("Known LocalDb instance not found!");
				}
			}

			return _sLocalDbInstanceName;
		}
	}

}
