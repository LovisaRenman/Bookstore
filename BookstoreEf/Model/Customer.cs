using System;
using System.Collections.Generic;
using BookstoreEf.Model;

namespace BookstoreEf;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phonenumber { get; set; }

    public virtual ICollection<BookReview1> BookReview1s { get; set; } = new List<BookReview1>();
}
