using System;

namespace Bookstore.Contracts
{
    public class BookCreated
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Cost { get; set; }
        public int InventoryAmount { get; set; }
    }
}