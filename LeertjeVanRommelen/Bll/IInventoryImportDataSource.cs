using System.Collections.Generic;

namespace LeertjeVanRommelen.Bll
{
    internal interface IInventoryImportDataSource
    {
        /// <summary>
        /// Can throw DataSourceUnavailableException
        /// </summary>
        /// <returns></returns>
        IEnumerable<InventoryImportDataItem> GetInventoryDataToImport();
    }
}