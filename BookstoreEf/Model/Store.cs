using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreEf.Model;

public partial class Store
{
    public int Id { get; set; }

    public string StoreName { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string StreetNumber { get; set; } = null!;

    public string Postcode { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [NotMapped]
    public string? StoreAdress { get; set; }
}
