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
        private readonly Lazy<Dictionary<int, VAT>> _vatsByPercentage;

        public Inventory(IDbSet<Product> products, IEnumerable<VAT> vats)
        {
            _products = products;
            _vatsByPercentage = new Lazy<Dictionary<int, VAT>>(() => vats.ToDictionary(x => x.Percentage));
        }

        public void Import(IInventoryImportDataSource importDataSource)
        {
            var inventoryItemsToImport = importDataSource.GetInventoryDataToImport().ToArray();


            var productsToImport = inventoryItemsToImport
                .Select(MapProduct)
                .ToArray();

            var skusToRemove = productsToImport.Select(x => x.Sku).ToArray();
            RemoveProductsWithSkus(skusToRemove);

            Console.WriteLine("Importing {0} new products", productsToImport.Count());
            _products.AddRange(productsToImport);
        }

        private void RemoveProductsWithSkus(IEnumerable<string> skusToRemove)
        {
            var productsToRemove = _products.Where(x => skusToRemove.Contains(x.Sku));
            
            Console.WriteLine("Deleting {0} old products", productsToRemove.Count());

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

        private Product MapDataFromImportItem(Product product, InventoryImportDataItem item)
        {
            MapVat(product, item);

            product.Price = item.Price*0.15m;
            product.Sku = item.Sku;
            product.ShortDescription = item.ShortDescription;
            product.FullDescription = item.FullDescription;
            product.Brand = item.Brand;
            product.Model = item.Model;

            return product;
        }

        private void MapVat(Product product, InventoryImportDataItem item)
        {
            VAT vat;
            if (_vatsByPercentage.Value.TryGetValue(item.VAT, out vat))
            {
                product.VATId = vat.Id;
            }
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