using System.IO;

namespace LeertjeVanRommelen.Services
{
    internal interface IServiceInventoryImportFromCsvFile
    {
        void ImportCsvFile(FileInfo fileinfo);
    }
}