﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _productSet = new Mock<IDbSet<Product>>();
            
            _importData= new List<InventoryImportDataItem>();
            _importDataSource = new Mock<IInventoryImportDataSource>();
            _importDataSource.Setup(x => x.GetInventoryDataToImport()).Returns(_importData);
            
            _subject = new Inventory(_productSet.Object);
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
    }
}
