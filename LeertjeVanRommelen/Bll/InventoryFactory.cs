using LeertjeVanRommelen.Dal;

namespace LeertjeVanRommelen.Bll
{
    internal class InventoryFactory : ICreateInventoryImporter
    {
        private readonly ILog _log;

        public InventoryFactory(ILog log)
        {
            _log = log;
        }

        public IImportInventory CreateInventoryImporter(IInventoryContext context)
        {
            return new Inventory(context.Products, context.Vats, _log);
        }
    }
}