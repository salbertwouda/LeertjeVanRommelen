using System.Data.Entity.Infrastructure;
using System.IO;
using LeertjeVanRommelen.Bll;
using LeertjeVanRommelen.Dal;

namespace LeertjeVanRommelen.Services
{
    internal class InventoryService : IServiceInventoryImportFromCsvFile
    {
        private readonly ICreateInventoryContext _inventoryContextFactory;
        private readonly ICreateInventoryImporter _inventoryImporterFactory;
        private readonly ICreateInventoryImportDataSource _importDataSourceFactory;

        public InventoryService(ICreateInventoryContext inventoryContextFactory, ICreateInventoryImporter inventoryImporterFactory, ICreateInventoryImportDataSource importDataSourceFactory)
        {
            _inventoryContextFactory = inventoryContextFactory;
            _inventoryImporterFactory = inventoryImporterFactory;
            _importDataSourceFactory = importDataSourceFactory;
        }

        public void ImportCsvFile(FileInfo fileinfo)
        {
            var datasource = _importDataSourceFactory.CreateCsvFileDataSource(fileinfo);
            ImportFromDataSource(datasource);
        }

        private void ImportFromDataSource(IInventoryImportDataSource datasource)
        {
            using (var context = _inventoryContextFactory.CreateInventoryContext())
            {
                var inventory = _inventoryImporterFactory.CreateInventoryImporter(context);
                inventory.Import(datasource);

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    throw new InventoryImportException("Persisting to database failed.", e);
                }
            }
        }
    }
}
