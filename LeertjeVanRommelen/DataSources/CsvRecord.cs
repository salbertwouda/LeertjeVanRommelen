namespace LeertjeVanRommelen.DataSources
{
    public class CsvRecord
    {
        public decimal PrIce { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int VAT { get; set; }
    }
}