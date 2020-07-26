using System;

namespace Bookstore.Audit
{
    public class UserAction
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public ActionType ActionType { get; set; }
    }
}