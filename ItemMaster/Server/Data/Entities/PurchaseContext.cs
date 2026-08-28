using Microsoft.Data.SqlClient;
using System.Data;

namespace ItemMaster.Server.Data.Entities
{
	public class PurchaseContext
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;
		public PurchaseContext(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("PurchaseConnection");
		}
		public IDbConnection CreateConnection()
		=> new SqlConnection(_connectionString);
	}
}
