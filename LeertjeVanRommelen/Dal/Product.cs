using System;

namespace LeertjeVanRommelen.Dal
{
    internal class Product : Entity
    {
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string MetaDescription { get; set; }
        public string Supplier { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime AvailableSince { get; set; }
        public string Sku { get; set; }

        public int StoreId { get; set; }
        public int ImageId { get; set; }
        public int ThumbnaiId { get; set; }
        public int CategoryId { get; set; }
        public int VATId { get; set; }
    }
}