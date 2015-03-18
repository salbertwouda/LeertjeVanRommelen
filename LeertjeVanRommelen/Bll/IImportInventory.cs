namespace LeertjeVanRommelen.Bll
{
    internal interface IImportInventory
    {
        /// <summary>
        /// May throw InventoryImportException
        /// </summary>
        /// <param name="importDataSource"></param>
        void Import(IInventoryImportDataSource importDataSource);
    }
}