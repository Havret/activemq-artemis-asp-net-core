using System;

namespace Bookstore
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Cost { get; set; }
        public int InventoryAmount { get; set; }
    }
}