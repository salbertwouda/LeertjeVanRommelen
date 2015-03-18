using System.IO;

namespace LeertjeVanRommelen.Bll
{
    internal interface ICreateInventoryImportDataSource
    {
        IInventoryImportDataSource CreateCsvFileDataSource(FileInfo fileinfo);
    }
}