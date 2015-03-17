using System;
using System.IO;
using System.Linq;
using LeertjeVanRommelen.DataSources;
using NUnit.Framework;

namespace Tests
{
    [TestFixture, Category("Integration")]
    public class CsvFileInventoryImportDataSourceTests
    {
        private CsvFileInventoryImportDataSource _subject;
        private FileInfo _fileInfo;
        private const string TestCsv = @"
Sku;Name;PrIce;ShortDescription;FullDescription;Brand;Model;VAT;ModelNumber;Color
1234894;naam;2495;Schoenuh;Vetta pata's;Henry's slippers;Normaal;2100;158;Yellow
1234895;naam2;2995;Schoenuh;Vetta pata's;Henry's slippers;Normaal;2100;158;Yellow
";

        [SetUp]
        public void Setup()
        {
            _fileInfo = new FileInfo(string.Format("CsvFileInventoryImportDataSourceTests-{0}.csv", Guid.NewGuid()));

            _subject = new CsvFileInventoryImportDataSource(_fileInfo);
        }

        [TearDown]
        public void TearDown()
        {
            if (_fileInfo.Exists)
            {
                File.Delete(_fileInfo.FullName);
            }
        }

        private void CreateFile()
        {
            File.WriteAllText(_fileInfo.FullName, TestCsv);
        }

        [Test]
        public void WhenFileExists_ThenImportData()
        {
            CreateFile();

            var result = _subject.GetInventoryDataToImport();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void WhenFileDoesNotExist_ThenReturnEmptyEnumerable()
        {
            var result = _subject.GetInventoryDataToImport();

            Assert.IsFalse(result.Any());
        }
    }
}