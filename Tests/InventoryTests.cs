using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LeertjeVanRommelen.Bll;
using LeertjeVanRommelen.Dal;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Tests
{
    [TestFixture]
    public class InventoryTests
    {
        private Mock<IDbSet<Product>> _productSet;
        private Inventory _subject;
        private List<InventoryImportDataItem> _importData;
        private Mock<IInventoryImportDataSource> _importDataSource;
        private Fixture _fixture;
        private List<Product> _products;
        private List<VAT> _vats;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _productSet = new Mock<IDbSet<Product>>();

            _products = new List<Product>();
            
            var productsQueryable = _products.AsQueryable();
            _productSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productsQueryable.Provider);
            _productSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productsQueryable.Expression);
            _productSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productsQueryable.ElementType);
            _productSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(productsQueryable.GetEnumerator());

            _vats = new List<VAT>();

            _importData= new List<InventoryImportDataItem>();
            _importDataSource = new Mock<IInventoryImportDataSource>();
            _importDataSource.Setup(x => x.GetInventoryDataToImport()).Returns(_importData);
            
            _subject = new Inventory(_productSet.Object, _vats);
        }

        public SetDefaultDataTestCase[] GetSetDefaultDataTestCases()
        {
            return new[]
            {
                new SetDefaultDataTestCase(x => x.StoreId == 1),
                new SetDefaultDataTestCase(x => x.Supplier == "Schoeisel BV"),
                new SetDefaultDataTestCase(x => DateTime.UtcNow.AddSeconds(-1) < x.AvailableSince && x.AvailableSince < DateTime.UtcNow.AddSeconds(1)),
                new SetDefaultDataTestCase(x => x.ImageId == 34),
                new SetDefaultDataTestCase(x => x.ThumbnaiId == 87),
                new SetDefaultDataTestCase(x => x.MetaDescription == null),
                new SetDefaultDataTestCase(x => x.CategoryId == 1),
            };
        }

        [TestCaseSource("GetSetDefaultDataTestCases")]
        public void WhenImport_ThenSetDefaultDataOnProduct(SetDefaultDataTestCase dataTestCase)
        {
            Arrange_AddOneImportDataItem();

            Import();

            AssertAddProductOnce(dataTestCase.Predicate);
        }

        public MapsDataFromItemTestCase[] GetImportMapDataFromImportItemTestCases()
        {
            return new[]
            {
                new MapsDataFromItemTestCase(i => p => p.Price == i.Price * 0.15m),
                new MapsDataFromItemTestCase(i => p => p.Sku == i.Sku),
                new MapsDataFromItemTestCase(i => p => p.ShortDescription == i.ShortDescription),
                new MapsDataFromItemTestCase(i => p => p.FullDescription == i.FullDescription),
                new MapsDataFromItemTestCase(i => p => p.Brand == i.Brand),
                new MapsDataFromItemTestCase(i => p => p.Model == i.Model),
            };
        }

        [TestCaseSource("GetImportMapDataFromImportItemTestCases")]
        public void WhenImport_ThenMapDataFromItemToProduct(MapsDataFromItemTestCase testCase)
        {
            var item = Arrange_AddOneImportDataItem();

            Import();

            AssertAddProductOnce(testCase.GetPredicate(item));
        }

        private InventoryImportDataItem Arrange_AddOneImportDataItem()
        {
            var inventoryImportDataItem = _fixture.Create<InventoryImportDataItem>();
            _importData.Add(inventoryImportDataItem);
            return inventoryImportDataItem;
        }

        private void Import()
        {
            _subject.Import(_importDataSource.Object);
        }

        public class SetDefaultDataTestCase
        {
            internal SetDefaultDataTestCase(Expression<Func<Product, bool>> predicate)
            {
                Predicate = predicate;
            }

            internal Expression<Func<Product, bool>> Predicate { get; private set; }

            public override string ToString()
            {
                return Predicate.ToString();
            }
        }

        public class MapsDataFromItemTestCase
        {
            private Expression<Func<InventoryImportDataItem, Expression<Func<Product, bool>>>> _getPredicate;

            internal MapsDataFromItemTestCase(Expression<Func<InventoryImportDataItem, Expression<Func<Product, bool>>>> getPredicate)
            {
                _getPredicate = getPredicate;
                GetPredicate = getPredicate.Compile();
            }

            internal Func<InventoryImportDataItem, Expression<Func<Product, bool>>> GetPredicate { get; private set; }

            public override string ToString()
            {
                return _getPredicate.ToString();
            }
        }

        private void AssertAddProductOnce(Expression<Func<Product, bool>> predicate)
        {
            _productSet.Verify(x => x.Add(It.Is(predicate)), Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(100)]
        public void WhenImportingExistingProducts_ThenRemoveThoseProductsFromDbSet(int amountOfExistingProducts)
        {
            var items = Enumerable.Range(0, amountOfExistingProducts).Select(x => {
                var item = Arrange_AddOneImportDataItem();
                item.Sku = "sku" + x;
                return item;
            }).ToArray();

            var productsToRemove = items.Select(x =>
            {
                var product = _fixture.Create<Product>();
                product.Sku = x.Sku;

                _products.Add(product);

                return product;
            }).ToArray();

            Import();

            foreach (var productToRemove in productsToRemove)
            {
                var capturedProduct = productToRemove;
                _productSet.Verify(x => x.Remove(capturedProduct), Times.Once);
            }
        }

        [Test]
        public void WhenImportItemHasExistingVat_ThenSetVatIdOnProduct()
        {
            const int percentage = 16;
            const int vatId = int.MaxValue;
            _vats.Add(new VAT
            {
                Id=vatId,
                Percentage = percentage
            });

            var item = Arrange_AddOneImportDataItem();
            item.VAT = percentage;

            Import();

            AssertAddProductOnce(x => x.VATId == vatId);
        }
    }
}
