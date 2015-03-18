using System.Data.Entity;
using System.Diagnostics;

namespace LeertjeVanRommelen.Dal
{
    internal class InventoryDbInitializer : CreateDatabaseIfNotExists<InventoryContext>
    {
        public override void InitializeDatabase(InventoryContext context)
        {
            base.InitializeDatabase(context);

#if DEBUG
            context.Database.Log = x => Debug.WriteLine(x);
#endif
        }

        protected override void Seed(InventoryContext context)
        {
            base.Seed(context);

            context.Vats.Add(new VAT {Name = "Normaal", Percentage = 2100});
        }
    }
}