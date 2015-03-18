using System.Data.Common;
using System.Data.Entity;

namespace LeertjeVanRommelen.Dal
{
    internal class InventoryContext : DbContext, IInventoryContext
    {
        public InventoryContext(string catalogName)
            : base(catalogName)
        {
        }

        public InventoryContext(DbConnection connection, ConnectionOwnedByContext connectionOwner)
            : base(connection, connectionOwner == ConnectionOwnedByContext.Yes)
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
