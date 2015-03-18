using System;
using System.IO;
using DryIoc;
using LeertjeVanRommelen.Bll;
using LeertjeVanRommelen.Dal;
using LeertjeVanRommelen.DataSources;
using LeertjeVanRommelen.Services;

namespace LeertjeVanRommelen
{
    class Program
    {
        private readonly ILog _log;
        private readonly IServiceInventoryImportFromCsvFile _importFromService;

        static void Main(string[] args)
        {
            var container = CreateContainer();
            var program = container.Resolve<Program>();
            program.Run();
        }

        private static Container CreateContainer()
        {
            var container = new Container();

            container.Register<Program>();
            container.Register<ILog, ConsoleLog>();
            container.Register<ICreateInventoryContext, InventoryContextFactory>();
            container.Register<ICreateInventoryImportDataSource, InventoryImportDataSourceFactory>();
            container.Register<ICreateInventoryImporter, InventoryFactory>();
            container.Register<IServiceInventoryImportFromCsvFile, InventoryService>();

            return container;
        }

        public Program(ILog log, IServiceInventoryImportFromCsvFile importFromService)
        {
            _log = log;
            _importFromService = importFromService;
        }

        public void Run()
        {
            do
            {
                var fileinfo = new FileInfo("Leverancier.csv");
                try
                {
                   _importFromService.ImportCsvFile(fileinfo);
                }
                catch (InventoryImportException e)
                {
                    _log.Error("Import failed");
                    _log.Error(e.ToString());
                }
            } while (Console.ReadLine() != "exit");
        }
    }
}
