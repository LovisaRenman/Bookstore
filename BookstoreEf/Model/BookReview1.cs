using System;
using System.Collections.Generic;

namespace BookstoreEf.Model;

public partial class BookReview1
{
    public int Id { get; set; }

    public string BookIsbn { get; set; } = null!;

    public int CustomerId { get; set; }

    public int? Rating { get; set; }

    public string? ReviewText { get; set; }

    public DateTime? ReviewDate { get; set; }

    public virtual Book BookIsbnNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
