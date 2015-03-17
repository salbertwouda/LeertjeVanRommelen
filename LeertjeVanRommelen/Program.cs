using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeertjeVanRommelen.Dal;

namespace LeertjeVanRommelen
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var context = new InventoryContext())
            {
                Console.WriteLine("amount of products: "+context.Products.Count());
            }

            Console.ReadKey();
        }
    }
}
