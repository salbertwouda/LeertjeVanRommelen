using System.Data.Entity;

namespace LeertjeVanRommelen.Dal
{
    internal class InventoryContext : DbContext, IInventoryContext
    {
        public InventoryContext()
            : base("Inventory")
        {
        }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<VAT> Vats { get; set; }

        void IInventoryContext.SaveChanges()
        {
            SaveChanges();
        }

        static InventoryContext()
        {
            Database.SetInitializer(new InventoryDbInitializer());
        }
    }
}
