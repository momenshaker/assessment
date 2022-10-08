namespace Product.Microservice.Dto
{
    public class CatalogDto
    {
        public int? id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal cost { get; set; }
        public string imageBase { get; set; }
    }
}
