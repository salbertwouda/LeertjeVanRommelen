using System;
using System.IO;
using DryIoc;
using LeertjeVanRommelen.Bll;
using LeertjeVanRommelen.Dal;
using LeertjeVanRommelen.DataSources;
using LeertjeVanRommelen.Services;

namespace LeertjeVanRommelen
{
    internal class Program
    {
        private const string DefaultCatalogName = "Inventory";
        private const string DefaultSourceFile = "SampleData.csv";

        private readonly ILog _log;
        private readonly IServiceInventoryImportFromCsvFile _importFromService;

        static void Main(string[] args)
        {
            var options = new ProgramOptions
            {
                SourceFile = DefaultSourceFile
            };
            options.InterpretArguments(args);

            var container = CreateContainer(options);
            var program = container.Resolve<Program>();
            program.Run(options);
        }
      
        private static Container CreateContainer(ProgramOptions options)
        {
            var container = new Container();

            container.Register<Program>();
            container.Register<ILog, ConsoleLog>();

            container.RegisterDelegate(
                c => string.IsNullOrWhiteSpace(options.ConnectionString)
                    ? new SqlCompactInventoryContextFactory(DefaultCatalogName, c.Resolve<ILog>()) as ICreateInventoryContext
                    : new SqlInventoryContextFactory(options.ConnectionString, c.Resolve<ILog>()) as ICreateInventoryContext
                  , Reuse.Transient);

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

        public void Run(ProgramOptions programOptions)
        {
            do
            {
                var fileinfo = new FileInfo(programOptions.SourceFile);
                try
                {
                   _importFromService.ImportCsvFile(fileinfo);
                }
                catch (InventoryImportException e)
                {
                    _log.Error("Import failed");
                    _log.Exception(e);
                }
            } while (Console.ReadLine() != "exit");
        }
    }
}
