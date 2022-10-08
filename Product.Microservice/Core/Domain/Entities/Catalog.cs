namespace Product.Microservice.Infrastructure.Entities
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string ImageBase { get; set; }
    }
}
