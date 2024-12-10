using BookstoreEf.Command;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel;

class AuthorViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;


    private ObservableCollection<Author> _authors;
    public ObservableCollection<Author> Authors
    {
        get => _authors; 
        set
        {            
            _authors = value;       
            RaisePropertyChanged();
        }
    }

    private Author? _selectedAuthor;
    public Author? SelectedAuthor
    {
        get => _selectedAuthor; 
        set 
        {            
            _selectedAuthor = value;
            RaisePropertyChanged();             
            ChangeTextVisibility();
            AddAuthorCommand.RaiseCanExecuteChanged();
            DeleteAuthorCommand.RaiseCanExecuteChanged();
        }
    }


    private bool _isDeleteButtonEnable;
    public bool IsDeleteButtonEnable
    {
        get => _isDeleteButtonEnable; 
        set
        {
            _isDeleteButtonEnable = value;
            RaisePropertyChanged();          
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

    public event EventHandler<Author> DeleteAuthorRequested;
    public DelegateCommand AddAuthorCommand { get; }
    public DelegateCommand DeleteAuthorCommand { get; }


    public AuthorViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
        IsDeleteButtonEnable = false;
        
        AddAuthorCommand = new DelegateCommand(AddAuthor);
        DeleteAuthorCommand = new DelegateCommand(DeleteAuthor, IsDeleteAuthorEnable);

        LoadAuthors();

        SelectedAuthor = Authors.FirstOrDefault();
        TextVisibility = Authors.Count > 0;
    }


    private void LoadAuthors()
    {
        using var db = new BookstoreContext();

        var authors = db.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();

        Authors = new ObservableCollection<Author>(authors);
    }

    private void AddAuthor(object? obj)
    {
        using var db = new BookstoreContext();

        Authors.Add(new Author() { FirstName = "<New Author>", LastName = string.Empty }); 
        
        SelectedAuthor = (Authors.Count > 0) ? Authors.Last() : Authors.FirstOrDefault();
       
        //db.SaveChanges()
    }

    private void DeleteAuthor(object? obj)
    {
        using var db = new BookstoreContext();

        if (SelectedAuthor != null) DeleteAuthorRequested.Invoke(this, SelectedAuthor);

        ChangeTextVisibility();

        //db.SaveChanges()
    }

    private bool IsDeleteAuthorEnable(object? obj) => IsDeleteButtonEnable = SelectedAuthor != null && Authors.Count > 0;

    private void ChangeTextVisibility() => TextVisibility = (Authors?.Count > 0) && SelectedAuthor != null;

}


