namespace LeertjeVanRommelen.Dal
{
    internal class InventoryContextFactory : ICreateInventoryContext
    {
        public IInventoryContext CreateInventoryContext()
        {
            return new InventoryContext();
        }
    }
}