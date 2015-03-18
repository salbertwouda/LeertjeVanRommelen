namespace LeertjeVanRommelen.Dal
{
    internal class SqlCompactInventoryContextFactory : ICreateInventoryContext
    {
        private readonly string _catalogName;
        private readonly ILog _log;

        public SqlCompactInventoryContextFactory(string catalogName, ILog log)
        {
            _catalogName = catalogName;
            _log = log;
        }

        public IInventoryContext CreateInventoryContext()
        {
            _log.Info("Creating InventoryContext for SqlCompact");
            _log.Info("CatalogName: {0}", _catalogName);

            return new InventoryContext(_catalogName);
        }
    }
}