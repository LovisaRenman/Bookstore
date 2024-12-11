namespace BookstoreEf.Model;

public partial class Inventory
{
    public int StoreId { get; set; }

    public string BookIsbn { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Book BookIsbnNavigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
