using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel;

class AuthorViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;


    private ObservableCollection<Author> _authors;
    public ObservableCollection<Author> Authors
    {
        get { return _authors; }
        set
        {
            if (_authors != value)
            {
                _authors = value;
                RaisePropertyChanged();
            }
        }
    }

    private Author _selectedAuthor;
    public Author SelectedAuthor
    {
        get { return _selectedAuthor; }
        set 
        {
            if (_selectedAuthor != value)
            {
                _selectedAuthor = value;
                RaisePropertyChanged();
            }
        }
    }


    public AuthorViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        LoadAuthors();
        GetAuthorName();

        _selectedAuthor = Authors?.FirstOrDefault();
    }


    public void LoadAuthors()
    {
        using var db = new BookstoreContext();
        var authors = db.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();

        Authors = new ObservableCollection<Author>(authors);
    }

    public void GetAuthorName()
    {
        foreach (var author in Authors)
        {
            author.Name = string.Join(" ", author.FirstName, author.LastName);
        }
    }
}
