using BookstoreEf.Command;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.ObjectModel;
using System.Windows;

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

    private Author? _newAuthor;
    public Author? NewAuthor
    {
        get => _newAuthor; 
        set 
        { 
            _newAuthor = value;
            RaisePropertyChanged();
        }
    }


    private bool _isAuthorViewVisible;
    public bool IsAuthorViewVisible
    {
        get =>_isAuthorViewVisible;
        set 
        { 
            _isAuthorViewVisible = value;
            RaisePropertyChanged();
        }
    }

    private bool _isAuthorMenuOptionEnable;
    public bool IsAuthorMenuOptionEnable
    {
        get => _isAuthorMenuOptionEnable;
        set
        {
            _isAuthorMenuOptionEnable = value;
            RaisePropertyChanged();
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
    public event EventHandler<Exception> FailedToAddAuthor;
    public event EventHandler<Exception> FailedToSaveAuthor;
    public event EventHandler SuccessfullySaveAuthor;

    public DelegateCommand AddAuthorCommand { get; }
    public DelegateCommand DeleteAuthorCommand { get; }
    public DelegateCommand SaveAuthorCommand { get; }
    public DelegateCommand SwitchToAuthorViewCommand { get; }


    public AuthorViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        IsDeleteButtonEnable = false;
        IsAuthorMenuOptionEnable = true;

        AddAuthorCommand = new DelegateCommand(AddAuthor);
        DeleteAuthorCommand = new DelegateCommand(DeleteAuthor, IsDeleteAuthorEnable);
        SaveAuthorCommand = new DelegateCommand(SaveAuthorInformation);
        SwitchToAuthorViewCommand = new DelegateCommand(StartAuthorView, IsAuthorViewEnable);

        LoadAuthors();

        SelectedAuthor = Authors.FirstOrDefault();
        TextVisibility = Authors.Count > 0;        
    }


    private void LoadAuthors()
    {
        using var db = new BookstoreContext();

        var authors = db.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();

        Authors = new ObservableCollection<Author>(authors);

        IsAuthorDeceased(); 
    }

    private void AddAuthor(object? obj)
    {
        using var db = new BookstoreContext();

        NewAuthor = new Author() { FirstName = "New Author" };
        db.Authors.Add(NewAuthor);

        try
        {
            db.SaveChanges();
        }
        catch (Exception e)
        {
            FailedToAddAuthor.Invoke(this, e);           
        }

        var authors = db.Authors.ToList();

        Authors = new ObservableCollection<Author>(authors);

        SelectedAuthor = Authors.LastOrDefault();

        IsAuthorDeceased(); 
    }

    private void DeleteAuthor(object? obj)
    {
        if (SelectedAuthor != null)
        {
            using var db = new BookstoreContext();

            DeleteAuthorRequested.Invoke(this, SelectedAuthor);

            var authors = db.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();

            Authors = new ObservableCollection<Author>(authors);

            ChangeTextVisibility();

            IsAuthorDeceased();
        }
    }

    private void IsAuthorDeceased()     
    {
        foreach (var author in Authors)
        {
            author.IsDeceased = (author.DateofDeath is null) ? false : true;
        }
    }

    public void SaveAuthorInformation(object? obj) 
    {
        using var db = new BookstoreContext();

        var author = db.Authors.FirstOrDefault(a => a.Id == SelectedAuthor.Id);

        try
        {
            if (author != null)
            {
                author.FirstName = SelectedAuthor.FirstName;
                author.LastName = SelectedAuthor.LastName;
                author.DateofBirth = SelectedAuthor.DateofBirth;
                author.DateofDeath = SelectedAuthor.DateofDeath;

                db.SaveChanges();

                SuccessfullySaveAuthor.Invoke(this, EventArgs.Empty);
            }    
        }
        catch (Exception e)
        {
            FailedToSaveAuthor.Invoke(this, e);
        }

        IsAuthorDeceased(); 
    } 

    private bool IsDeleteAuthorEnable(object? obj) => IsDeleteButtonEnable = SelectedAuthor != null && Authors.Count > 0;

    private void StartAuthorView(object? obj)
    {
        IsAuthorMenuOptionEnable = false;

        IsAuthorViewVisible = true;
        mainWindowViewModel.BookViewModel.IsBookViewVisible = false;
        mainWindowViewModel.StoreInventoryViewModel.IsStoreInventoryViewVisible = false;

        UpdateCommandStates();
    }

    private bool IsAuthorViewEnable(object? obj) => IsAuthorMenuOptionEnable = !IsAuthorViewVisible;

    private void ChangeTextVisibility() => TextVisibility = (Authors?.Count > 0) && SelectedAuthor != null;

    private void UpdateCommandStates()
    {
        SwitchToAuthorViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.StoreInventoryViewModel.SwitchToStoreInventoryViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.BookViewModel.SwitchToBookViewCommand.RaiseCanExecuteChanged();
    }  

}


