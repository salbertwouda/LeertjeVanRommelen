using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LeertjeVanRommelen.Dal;

namespace LeertjeVanRommelen.Bll
{
    internal class Inventory : IImportInventory
    {
        private readonly IDbSet<Product> _products;

        public Inventory(IDbSet<Product> products)
        {
            _products = products;
        }

        public void Import(IInventoryImportDataSource importDataSource)
        {
            var productsToImport = importDataSource.GetInventoryDataToImport()
                .Select(MapProduct)
                .ToArray();

            var skusToRemove = productsToImport.Select(x => x.Sku).ToArray();
            RemoveProductsWithSkus(skusToRemove);

            _products.AddRange(productsToImport);
        }

        private void RemoveProductsWithSkus(IEnumerable<string> skusToRemove)
        {
            var productsToRemove = _products.Where(x => skusToRemove.Contains(x.Sku));
            foreach (var productToRemove in productsToRemove)
            {
                _products.Remove(productToRemove);
            }
        }

        private Product MapProduct(InventoryImportDataItem item)
        {
            var product = CreateProductWithDefaultValues();
            return MapDataFromImportItem(product, item);
        }

        private static Product MapDataFromImportItem(Product product, InventoryImportDataItem item)
        {
            product.Price = item.Price*0.15m;
            product.Sku = item.Sku;
            product.ShortDescription = item.ShortDescription;
            product.FullDescription = item.FullDescription;
            product.Brand = item.Brand;
            product.Model = item.Model;

            return product;
        }

        private Product CreateProductWithDefaultValues()
        {
            return new Product
            {
                StoreId = 1,
                Supplier = "Schoeisel BV",
                AvailableSince = DateTime.UtcNow,
                ImageId = 34,
                ThumbnaiId = 87,
                MetaDescription = null,
                CategoryId = 1,
            };
        }
    }
}