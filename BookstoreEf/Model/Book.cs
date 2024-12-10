using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using BookstoreEf.Model;

namespace BookstoreEf;

public partial class Book : INotifyPropertyChanged
{
    public Book(Book original)
    {
        Isbn = original.Isbn;
        BookTitle = original.BookTitle;
        Language = original.Language;
        Price = original.Price;
        Pages = original.Pages;
        PublishDate = original.PublishDate;
        WeightInGrams = original.WeightInGrams;
        GenreId = original.GenreId;
        Inventories = original.Inventories;
        Publisher = original.Publisher;
        Authors = original.Authors;
    }

    public Book()
    {
        
    }

    private string bookTitle = null!;
    private Genre? genre;
    private Publisher? publisher;
    private string isbn = null!;
    private string? language;
    private decimal price;
    private DateOnly publishDate;
    private int pages;
    private int? weightInGrams;

    public string Isbn
    {
        get => isbn;
        set
        {
            isbn = value;
            RaisePropertyChanged();
        }
    }

    public string BookTitle 
    { 
        get => bookTitle; 
        set
        {
            bookTitle = value;
            RaisePropertyChanged();
        }
    }
    public string? Language 
    { 
        get => language; 
        set
        {
            language = value;
            RaisePropertyChanged();

        }
    }

    public decimal Price 
    { 
        get => price;
        set
        {
            price = value;
            RaisePropertyChanged();
        }
    }

    public DateOnly PublishDate
    {
        get => publishDate;
        set
        {
            publishDate = value;
            RaisePropertyChanged();
        }
    }

    public int Pages 
    {
        get => pages;
        set
        {
            pages = value;
            RaisePropertyChanged();
        } 
    }

    public int? WeightInGrams 
    {
        get => weightInGrams; 
        set
        {
            weightInGrams = value;
            RaisePropertyChanged();
        } 
    }

    public int? GenreId { get; set; }

    public int? PublisherId { get; set; }

    public virtual ICollection<BookReview1> BookReview1s { get; set; } = new List<BookReview1>();

    public virtual Genre? Genre { get => genre; 
        set 
        {
            genre = value;
            RaisePropertyChanged();
        } 
    }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Publisher? Publisher
    {
        get => publisher;
        set
        {
            publisher = value;
            RaisePropertyChanged();
        }
    }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();


    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
