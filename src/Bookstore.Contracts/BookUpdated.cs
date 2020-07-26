using System;

namespace Bookstore.Contracts
{
    public class BookUpdated
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Cost { get; set; }
        public int InventoryAmount { get; set; }
        public Guid UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}