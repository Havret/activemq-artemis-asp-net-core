using System;

namespace Bookstore.Commands
{
    public class UpdateBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Cost { get; set; }
        public int InventoryAmount { get; set; }
        public Guid UserId { get; set; }
    }
}