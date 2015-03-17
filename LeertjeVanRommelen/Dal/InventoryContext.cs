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
        public IDbSet<VAT> Vats { get; set; }

        static InventoryContext()
        {
            Database.SetInitializer(new InventoryDbInitializer());
        }
    }
}
