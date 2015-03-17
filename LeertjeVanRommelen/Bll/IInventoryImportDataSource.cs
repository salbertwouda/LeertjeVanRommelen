using System.Collections.Generic;

namespace LeertjeVanRommelen.Bll
{
    internal interface IInventoryImportDataSource
    {
        IEnumerable<InventoryImportDataItem> GetInventoryDataToImport();
    }
}