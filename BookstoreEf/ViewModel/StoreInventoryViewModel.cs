using BookstoreEf.Command;
using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel;

class StoreInventoryViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;


    private BookInventoryTranslate _selectedBookTitle;
    public BookInventoryTranslate SelectedBookTitle
    {
        get => _selectedBookTitle; 
        set 
        { 
            _selectedBookTitle = value;
            RaisePropertyChanged();

            IsDeleteBookOptionEnable = _selectedBookTitle != null;
            IsUpdateBookQuantityEnable = _selectedBookTitle != null;
        }
    }


    private ObservableCollection<BookInventoryTranslate> _booksInSelectedStore; 
    public ObservableCollection<BookInventoryTranslate> BooksInSelectedStore
    {
        get => _booksInSelectedStore;
        set
        {
            _booksInSelectedStore = value;
            RaisePropertyChanged();
            RaisePropertyChanged("Books");
            DeleteBookCommand.RaiseCanExecuteChanged();
            SaveInventoryCommand.RaiseCanExecuteChanged();
        }
    }

    private ObservableCollection<Book> _books;
    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            _books = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Store> _stores;
    public ObservableCollection<Store> Stores
    {
        get => _stores; 
        set
        {
            if (_stores != value)
            {
                _stores = value;
                RaisePropertyChanged();
            }
        }
    }

    private Store _selectedStore;
    public Store SelectedStore
    {
        get => _selectedStore; 
        set
        {
            if (_selectedStore != value)
            {
                _selectedStore = value;
                RaisePropertyChanged();
                UpdateBooksForSelectedStore();
            }
        }
    }


    private bool _isDeleteBookOptioniEnable;
    public bool IsDeleteBookOptionEnable
    {
        get => _isDeleteBookOptioniEnable;
        set
        {
            _isDeleteBookOptioniEnable = value;
            RaisePropertyChanged();
        }
    }

    private bool _isStoreInventoryViewVisible;
    public bool IsStoreInventoryViewVisible
    {
        get => _isStoreInventoryViewVisible; 
        set 
        { 
            _isStoreInventoryViewVisible = value;
            RaisePropertyChanged();
        }
    }


    private bool _isStoreInventoryMenuOptionEnable;
    public bool IsStoreInventoryMenuOptionEnable
    {
        get => _isStoreInventoryMenuOptionEnable;
        set
        {
            _isStoreInventoryMenuOptionEnable = value;
            RaisePropertyChanged();
        }
    }

    private bool _isUpdateBookQuantityEnable ;
    public bool IsUpdateBookQuantityEnable
    {
        get => _isUpdateBookQuantityEnable; 
        set 
        { 
            _isUpdateBookQuantityEnable = value;
            RaisePropertyChanged();
        }
    }


    private decimal _totalInventoryValue; 
    public decimal TotalInventoryValue
    {
        get => _totalInventoryValue;
        set
        {
            _totalInventoryValue = value;
            RaisePropertyChanged();
        }
    }

    public event EventHandler CloseAddBookToSelectedStoreDialog;
    public event EventHandler CloseManageInventoryDialog;
    public event EventHandler<BookInventoryTranslate> DeleteBookFromStoreRequested;
    public event EventHandler OpenInventoryDialog;
    public event EventHandler OpenAddBookToStoreDialog;
    public event EventHandler InventoryUpdateSource;

    public DelegateCommand CloseAddBookToStoreCommand { get; }
    public DelegateCommand CloseManageInventoryCommand { get; }
    public DelegateCommand DeleteBookCommand { get; }
    public DelegateCommand OpenManageInventoryCommand { get; }
    public DelegateCommand OpenAddBooktitleToStoreCommand { get; }
    public DelegateCommand SaveInventoryCommand { get; set; }
    public DelegateCommand SwitchToStoreInventoryViewCommand { get; }

    public StoreInventoryViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        IsStoreInventoryViewVisible = true;

        CloseAddBookToStoreCommand = new DelegateCommand(CloseAddBook);
        CloseManageInventoryCommand = new DelegateCommand(CloseInventory);
        DeleteBookCommand = new DelegateCommand(DeleteBook);
        OpenManageInventoryCommand = new DelegateCommand(OpenInventory);
        OpenAddBooktitleToStoreCommand = new DelegateCommand(OpenAddBook);
        SaveInventoryCommand = new DelegateCommand(SaveInventory);
        SwitchToStoreInventoryViewCommand = new DelegateCommand(StartInventoryView, IsInventoryViewEnable);

        //LoadBooks();
        LoadStores();
        GetStoreAdress();

        SelectedStore = Stores?.FirstOrDefault();
    }

    private void CloseAddBook(object obj) => CloseAddBookToSelectedStoreDialog.Invoke(this, EventArgs.Empty);

    private void CloseInventory(object obj) => CloseManageInventoryDialog.Invoke(this, EventArgs.Empty);

    private void DeleteBook(object? obj) => DeleteBookFromStoreRequested.Invoke(this, SelectedBookTitle);

    private void GetStoreAdress()
    {
        foreach (var store in Stores)
        {
            store.StoreAdress = string.Join(", " , string.Join(" ", store.Street, store.StreetNumber), store.Postcode, store.City, store.Country);
        }
    }

    private bool IsInventoryViewEnable(object? obj) => IsStoreInventoryMenuOptionEnable = !IsStoreInventoryViewVisible;

    

    private void LoadStores() 
    {
        using var db = new BookstoreContext();
        var stores = db.Stores.ToList();

        Stores = new ObservableCollection<Store>(stores);
    }

    private void OpenAddBook(object obj) => OpenAddBookToStoreDialog.Invoke(this, EventArgs.Empty);

    private void OpenInventory(object? obj) => OpenInventoryDialog.Invoke(this, EventArgs.Empty);
    
    private void SaveInventory(object obj)
    {
        InventoryUpdateSource.Invoke(this, EventArgs.Empty);
        CloseManageInventoryDialog.Invoke(this, EventArgs.Empty);
    }
    
    private void StartInventoryView(object? obj)
    {
        IsStoreInventoryMenuOptionEnable = false;

        IsStoreInventoryViewVisible = true;
        mainWindowViewModel.AuthorViewModel.IsAuthorViewVisible = false;
        mainWindowViewModel.BookViewModel.IsBookViewVisible = false;
       
        UpdateCommandStates();
    } 
    
    private void UpdateBooksForSelectedStore()
    {
        if (SelectedStore == null)
        {
            BooksInSelectedStore = new ObservableCollection<BookInventoryTranslate>();
            TotalInventoryValue = 0;            
            return;
        }

        using (var db = new BookstoreContext())
        {
            var booksInStore = db.Inventories
                .Where(i => i.StoreId == SelectedStore.Id)
                .Select(i => new BookInventoryTranslate
                {
                    BookTitle = i.BookIsbnNavigation.BookTitle,
                    Quantity = i.Quantity,
                    Price = i.BookIsbnNavigation.Price
                })
                .ToList();

            BooksInSelectedStore = new ObservableCollection<BookInventoryTranslate>(booksInStore);
            LoadBooks(BooksInSelectedStore);
        }

        TotalInventoryValue = BooksInSelectedStore.Sum(book => book.TotalPrice);
    }

    public void LoadBooks(ObservableCollection<BookInventoryTranslate> booksInSelectedStore)
    {
        using var db = new BookstoreContext();
        var books = db.Books.ToList();
    }

    private void UpdateCommandStates()
    {        
        SwitchToStoreInventoryViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.AuthorViewModel.SwitchToAuthorViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.BookViewModel.SwitchToBookViewCommand.RaiseCanExecuteChanged();
    }

}
