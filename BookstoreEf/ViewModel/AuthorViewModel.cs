using BookstoreEf.Command;
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
                ChangeTextVisibility();
            }
        }
    }

    private Author? _selectedAuthor;
    public Author? SelectedAuthor
    {
        get { return _selectedAuthor; }
        set 
        {
            if (_selectedAuthor != value)
            {
                _selectedAuthor = value;
                RaisePropertyChanged();
                ChangeTextVisibility();
            }
        }
    }


    private bool _textVisibility;
    public bool TextVisibility
    {
        get => _textVisibility;
        set
        {
            _textVisibility = value;
            RaisePropertyChanged();
        }
    }


    public DelegateCommand AddAuthorCommand { get; }
    public DelegateCommand DeleteAuthorCommand { get; }


    public AuthorViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        LoadAuthors();
        GetAuthorName();
        ChangeTextVisibility();
        
        AddAuthorCommand = new DelegateCommand(AddAuthor);
        DeleteAuthorCommand = new DelegateCommand(AddAuthor);
    }


    private void LoadAuthors()
    {
        using var db = new BookstoreContext();

        var authors = db.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();

        Authors = new ObservableCollection<Author>(authors);
    }

    private void GetAuthorName()
    {
        foreach (var author in Authors)
        {
            author.Name = string.Join(" ", author.FirstName, author.LastName);
        }
    }

    private void AddAuthor(object? obj) 
    {
        // DateOnly har ingen motsvarande nullvärde som string.Empty?
        Authors.Add(new Author(string.Empty, string.Empty, DateOnly.MinValue, DateOnly.MinValue)); 
        SelectedAuthor = (Authors.Count > 0) ? Authors.Last() : Authors.FirstOrDefault();

        UpdateCommandStates();
    }
    private void DeleteAuthor(object? obj)
    {       
        Authors.Remove(SelectedAuthor);
        SelectedAuthor = (Authors.Count > 0) ? Authors.Last() : Authors.FirstOrDefault();

        UpdateCommandStates();
    }


    private void ChangeTextVisibility() => TextVisibility = (Authors?.Count > 0) && SelectedAuthor != null;




    private void UpdateCommandStates()
    {
        ChangeTextVisibility();

        AddAuthorCommand.RaiseCanExecuteChanged();
        DeleteAuthorCommand.RaiseCanExecuteChanged();
    }

}


