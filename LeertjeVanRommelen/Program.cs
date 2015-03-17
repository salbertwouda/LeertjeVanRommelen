using System;
using System.IO;
using LeertjeVanRommelen.Bll;
using LeertjeVanRommelen.Dal;
using LeertjeVanRommelen.DataSources;

namespace LeertjeVanRommelen
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                var fileinfo = new FileInfo("Leverancier.csv");
                ImportCsvFile(fileinfo);
            } while (Console.ReadLine() != "exit");
        }

        private static void ImportCsvFile(FileInfo fileinfo)
        {
            if (!fileinfo.Exists)
            {
                Console.WriteLine("File {0} does not exist, nothing to import", fileinfo.FullName);
                return;
            }
            var dataSource = new CsvFileInventoryImportDataSource(fileinfo);
            ImportInventory(dataSource);
        }

        private static void ImportInventory(CsvFileInventoryImportDataSource dataSource)
        {
            using (var context = new InventoryContext())
            {
                var inventory = new Inventory(context.Products, context.Vats);
                inventory.Import(dataSource);

                context.SaveChanges();
            }
        }
    }
}
