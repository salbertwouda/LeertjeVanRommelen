using System;
using System.Data.Entity;

namespace LeertjeVanRommelen.Dal
{
    internal interface IInventoryContext : IDisposable
    {
        IDbSet<Product> Products { get; set; }
        IDbSet<VAT> Vats { get; set; }

        void SaveChanges();
    }
}