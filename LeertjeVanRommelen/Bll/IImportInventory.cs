namespace LeertjeVanRommelen.Bll
{
    internal interface IImportInventory
    {
        void Import(IInventoryImportDataSource importDataSource);
    }
}