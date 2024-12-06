using System;
using System.Collections.Generic;

namespace BookstoreEf.Model;

public partial class TitlesByAuthor
{
    public string Author { get; set; } = null!;

    public string Age { get; set; } = null!;

    public int? Titles { get; set; }

    public int? ExemplarsInStore { get; set; }

    public string InventoryValue { get; set; } = null!;
}
