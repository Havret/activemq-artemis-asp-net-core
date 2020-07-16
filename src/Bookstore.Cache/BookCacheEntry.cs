using System;

namespace Bookstore.Cache
{
    public class BookCacheEntry
    {
        public BookCacheEntry(Guid id, string title, string author, decimal cost)
        {
            Id = id;
            Title = title;
            Author = author;
            Cost = cost;
        }

        public Guid Id { get;  }
        public string Title { get;  }
        public string Author { get;  }
        public decimal Cost { get;  }
    }
}