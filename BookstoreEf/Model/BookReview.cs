using System;
using System.Collections.Generic;

namespace BookstoreEf.Model;

public partial class BookReview
{
    public string BookTitle { get; set; } = null!;

    public string GenreName { get; set; } = null!;

    public string? StarRatingsOutOf5 { get; set; }

    public double? AverageRatingOutOf5 { get; set; }

    public int? Reviews { get; set; }
}
