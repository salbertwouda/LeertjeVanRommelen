using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using LeertjeVanRommelen.Bll;

namespace LeertjeVanRommelen.DataSources
{
    internal class CsvFileInventoryImportDataSource : IInventoryImportDataSource
    {
        private readonly FileInfo _fileInfo;

        public CsvFileInventoryImportDataSource(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public IEnumerable<InventoryImportDataItem> GetInventoryDataToImport()
        {
            return ReadRecordsFromFile()
                .Select(Map)
                .ToArray();
        }

        private IEnumerable<CsvRecord> ReadRecordsFromFile()
        {
            var csvConfiguration = new CsvConfiguration { Delimiter = ";" };

            try
            {
                using (var fileReader = File.OpenText(_fileInfo.FullName))
                using (var csvReader = new CsvReader(fileReader, csvConfiguration))
                {
                    return csvReader.GetRecords<CsvRecord>().ToArray();
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new DataSourceUnavailableException(e, GetType().Name);
            }
            catch (PathTooLongException e)
            {
                throw new DataSourceUnavailableException(e, GetType().Name);
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DataSourceUnavailableException(e, GetType().Name);
            }
            catch (FileNotFoundException e)
            {
                throw new DataSourceUnavailableException(e, GetType().Name);
            }
            catch (NotSupportedException e)
            {
                throw new DataSourceUnavailableException(e, GetType().Name);
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