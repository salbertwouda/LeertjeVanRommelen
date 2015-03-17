using System.Data.Entity;

namespace LeertjeVanRommelen.Dal
{
    internal class InventoryContext : DbContext
    {
        public InventoryContext()
            : base("Inventory")
        {
        }
        public IDbSet<Product> Products { get; set; }

        static InventoryContext()
        {
            Database.SetInitializer(new InventoryDbInitializer());
        }
    }
}
