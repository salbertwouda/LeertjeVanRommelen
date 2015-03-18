using System.Data.SqlClient;

namespace LeertjeVanRommelen.Dal
{
    internal class SqlInventoryContextFactory : ICreateInventoryContext
    {
        private readonly string _connectionString;
        private readonly ILog _log;

        public SqlInventoryContextFactory(string connectionString, ILog log)
        {
            _connectionString = connectionString;
            _log = log;
        }

        public IInventoryContext CreateInventoryContext()
        {
            _log.Info("Creating InventoryContext for Sql");
            _log.Info("ConnectionString: {0}", _connectionString);

            return new InventoryContext(
                new SqlConnection(_connectionString),
                ConnectionOwnedByContext.Yes
            );
        }
    }
}