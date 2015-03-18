using System.IO;
using LeertjeVanRommelen.Bll;

namespace LeertjeVanRommelen.DataSources
{
    internal class InventoryImportDataSourceFactory : ICreateInventoryImportDataSource
    {
        public IInventoryImportDataSource CreateCsvFileDataSource(FileInfo fileinfo)
        {
            return new CsvFileInventoryImportDataSource(fileinfo);
        }
    }
}