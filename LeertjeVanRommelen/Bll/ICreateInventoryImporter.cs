using LeertjeVanRommelen.Dal;

namespace LeertjeVanRommelen.Bll
{
    internal interface ICreateInventoryImporter
    {
        IImportInventory CreateInventoryImporter(IInventoryContext context);
    }
}