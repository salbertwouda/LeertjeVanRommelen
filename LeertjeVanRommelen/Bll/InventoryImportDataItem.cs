namespace LeertjeVanRommelen.Bll
{
    internal class InventoryImportDataItem
    {
        
        public decimal Price { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int VAT { get; set; }
    }
}