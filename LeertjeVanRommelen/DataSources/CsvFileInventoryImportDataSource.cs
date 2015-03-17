using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using LeertjeVanRommelen.Bll;

namespace LeertjeVanRommelen.DataSources
{
    class CsvFileInventoryImportDataSource : IInventoryImportDataSource
    {
        private readonly FileInfo _fileInfo;

        public CsvFileInventoryImportDataSource(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public IEnumerable<InventoryImportDataItem> GetInventoryDataToImport()
        {
            if (!_fileInfo.Exists)
            {
                Console.WriteLine("Warning: trying to import inventory but file not found {0}", _fileInfo.FullName);
                return Enumerable.Empty<InventoryImportDataItem>();
            }

            return ReadRecordsFromFile()
                .Select(Map)
                .ToArray();
        }

        private IEnumerable<CsvRecord> ReadRecordsFromFile()
        {
            var csvConfiguration = new CsvConfiguration { Delimiter = ";" };
            using (var fileReader = File.OpenText(_fileInfo.FullName))
            using (var csvReader = new CsvReader(fileReader, csvConfiguration))
            {
                while (csvReader.Read())
                {
                    yield return csvReader.GetRecord<CsvRecord>();
                }
            }
        }

        private InventoryImportDataItem Map(CsvRecord arg)
        {
            return new InventoryImportDataItem()
            {
                Brand = arg.Brand,
                FullDescription = arg.FullDescription,
                Model = arg.Model,
                Price = arg.PrIce,
                ShortDescription = arg.ShortDescription,
                Sku = arg.Sku,
                VAT = arg.VAT
            };
        }
    }
}