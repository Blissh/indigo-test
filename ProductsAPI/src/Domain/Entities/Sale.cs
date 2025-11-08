namespace ProductsAPI.src.Domain.Entities
{
    public class Sale
    {
        private readonly List<SaleItems> _items = new List<SaleItems>();
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public IReadOnlyCollection<SaleItems> Items => _items;
        
    }
}
