using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace BookstoreEf;

public partial class Author : INotifyPropertyChanged
{
    public int Id { get; set; }

    private string? _firstName;
    public string? FirstName
    {
        get =>_firstName; 
        set 
        { 
            _firstName = value;
            RaisePropertyChanged(); 
            RaisePropertyChanged(nameof(Name)); 
        }
    }

    private string? _lastName;
    public string? LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(Name));
        }
    }

    public DateOnly? DateofBirth { get; set; }

    public DateOnly? DateofDeath { get; set; }

    public virtual ICollection<Book> BookIsbns { get; set; } = new List<Book>();


    [NotMapped]
    public string? Name => $"{FirstName} {LastName}";

    [NotMapped]
    public bool IsDeceased { get; set; }


    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
