namespace DotNet_Header_Footer.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public decimal Price { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }
    }
}