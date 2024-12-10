using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using BookstoreEf.Model;

namespace BookstoreEf;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string BookTitle { get; set; } = null!;

    public string? Language { get; set; }

    public decimal Price { get; set; }

    public DateOnly PublishDate { get; set; }

    public int Pages { get; set; }

    public int? WeightInGrams { get; set; }

    public int? GenreId { get; set; }

    public int? PublisherId { get; set; }

    public virtual ICollection<BookReview1> BookReview1s { get; set; } = new List<BookReview1>();

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();


    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
