using System.Data.Entity;
using System.Diagnostics;

namespace LeertjeVanRommelen.Dal
{
    internal class InventoryDbInitializer : DropCreateDatabaseAlways<InventoryContext>
    {
        public override void InitializeDatabase(InventoryContext context)
        {
            base.InitializeDatabase(context);

#if DEBUG
            context.Database.Log = x => Debug.WriteLine(x);
#endif
        }
    }
}