using BookstoreEf.Command;
using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Automation;

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

    private BookInventoryTranslate _newBook;

    public BookInventoryTranslate NewBook
    {
        get => _newBook; 
        set 
        { 
            _newBook = value;
            RaisePropertyChanged();
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
            RaisePropertyChanged("TotalInventoryValue");
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

    private Book _selectedBook;

    public Book SelectedBook
    {
        get { return _selectedBook; }
        set { _selectedBook = value; }
    }

    private int _quantity;

    public int SelectedBookQuantity
    {
        get { return _quantity; }
        set { _quantity = value; }
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

    private bool _isUpdateBookQuantityEnable;
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
    public event EventHandler<Exception> FailedDbUpdate;

    public DelegateCommand CloseAddBookToStoreCommand { get; }
    public DelegateCommand CloseManageInventoryCommand { get; }
    public DelegateCommand DeleteBookCommand { get; }
    public DelegateCommand OpenManageInventoryCommand { get; }
    public DelegateCommand OpenAddBooktitleToStoreCommand { get; }
    public DelegateCommand SaveInventoryCommand { get; set; }
    public DelegateCommand SwitchToStoreInventoryViewCommand { get; }
    public DelegateCommand SaveNewBookToSelectedStoreCommand { get; }

    public StoreInventoryViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        IsStoreInventoryViewVisible = true;

        CloseAddBookToStoreCommand = new DelegateCommand(CloseAddBookDialog);
        CloseManageInventoryCommand = new DelegateCommand(CloseInventoryDialog);
        DeleteBookCommand = new DelegateCommand(DeleteBook);
        OpenManageInventoryCommand = new DelegateCommand(OpenInventory);
        OpenAddBooktitleToStoreCommand = new DelegateCommand(OpenAddBookToSelectedStore);
        SaveInventoryCommand = new DelegateCommand(SaveInventory);
        SaveNewBookToSelectedStoreCommand = new DelegateCommand(SaveNewBookToSelectedStore);
        SwitchToStoreInventoryViewCommand = new DelegateCommand(StartInventoryView, IsInventoryViewEnable);

        LoadStores();
        GetStoreAdress();

        SelectedStore = Stores?.FirstOrDefault();
    }

    private void CloseAddBookDialog(object obj) => CloseAddBookToSelectedStoreDialog.Invoke(this, EventArgs.Empty);

    private void CloseInventoryDialog(object obj) => CloseManageInventoryDialog.Invoke(this, EventArgs.Empty);

    private void DeleteBook(object? obj)
    {
        DeleteBookFromStoreRequested.Invoke(this, SelectedBookTitle);
        UpdateTotalInventoryValue();
    } 

    private void GetStoreAdress()
    {
        foreach (var store in Stores)
        {
            store.StoreAdress = string.Join(", " , string.Join(" ", store.Street, store.StreetNumber), store.Postcode, store.City, store.Country);
        }
    }

    private bool IsInventoryViewEnable(object? obj) => IsStoreInventoryMenuOptionEnable = !IsStoreInventoryViewVisible;    

    public void LoadBooks(ObservableCollection<BookInventoryTranslate> booksInSelectedStore)
    {
        using var db = new BookstoreContext();
        var books = db.Books.ToList();

        var booksToBeRemoved = new List<Book>();

        foreach (var book in books)
        {
            foreach (var inventory in booksInSelectedStore)
            {
                if (book.BookTitle == inventory.BookIsbn)
                {
                    booksToBeRemoved.Add(book);
                    break;
                }
            }
        }

        foreach (var book in booksToBeRemoved)
        {
            books.Remove(book);
        }

        Books = new ObservableCollection<Book>(books);        

    }

    private void LoadStores() 
    {
        using var db = new BookstoreContext();
        var stores = db.Stores.ToList();

        Stores = new ObservableCollection<Store>(stores);
    }

    private void SaveNewBookToSelectedStore(object? obj)
    {
        using var db = new BookstoreContext();

        NewBook = new BookInventoryTranslate()
        {
            BookIsbn = SelectedBook.BookTitle,
            Quantity = SelectedBookQuantity, 
            Price = SelectedBook.Price,
        };

        db.Inventories.Add(new Inventory() {StoreId = SelectedStore.Id, BookIsbn = SelectedBook.Isbn, Quantity = NewBook.Quantity });

        try
        {
            db.SaveChanges();
        }
        catch (Exception e)
        {
            FailedDbUpdate.Invoke(this, e);
        }

        BooksInSelectedStore.Add(NewBook);

        SelectedBookTitle = NewBook;

        CloseAddBookToSelectedStoreDialog.Invoke(this, EventArgs.Empty);

        UpdateTotalInventoryValue();
    }

    private void OpenAddBookToSelectedStore(object obj)
    {
        OpenAddBookToStoreDialog.Invoke(this, EventArgs.Empty);
    }

    private void OpenInventory(object? obj) => OpenInventoryDialog.Invoke(this, EventArgs.Empty);
    
    private void SaveInventory(object obj)
    {
        InventoryUpdateSource.Invoke(this, EventArgs.Empty);
        CloseManageInventoryDialog.Invoke(this, EventArgs.Empty);
        UpdateTotalInventoryValue();

        using var db = new BookstoreContext();
        Inventory inventory = new Inventory()
        {
            BookIsbn = SelectedBookTitle.BookIsbn,
            Quantity = SelectedBookTitle.Quantity,
            StoreId = SelectedStore.Id
            
        };


        var inventoryEntry = db.Entry(db.Inventories.FirstOrDefault(i => i.BookIsbn == inventory.BookIsbn && i.StoreId == inventory.StoreId));
        inventoryEntry.CurrentValues.SetValues(inventory);

        try
        {
            db.SaveChanges();
        }
        catch (Exception e)
        {
            FailedDbUpdate.Invoke(this, e);
        }
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

        using var db = new BookstoreContext();
        
        var booksInStore = db.Inventories
            .Where(i => i.StoreId == SelectedStore.Id)
            .Select(i => new BookInventoryTranslate
            {
                BookIsbn = i.BookIsbnNavigation.Isbn,
                BookTitle = i.BookIsbnNavigation.BookTitle,
                Quantity = i.Quantity,
                Price = i.BookIsbnNavigation.Price
            })
            .ToList();

        BooksInSelectedStore = new ObservableCollection<BookInventoryTranslate>(booksInStore);
            
        LoadBooks(BooksInSelectedStore);
        
        UpdateTotalInventoryValue();
    }

    public void UpdateTotalInventoryValue() => TotalInventoryValue = BooksInSelectedStore.Sum(book => book.TotalPrice);

    private void UpdateCommandStates()
    {        
        SwitchToStoreInventoryViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.AuthorViewModel.SwitchToAuthorViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.BookViewModel.SwitchToBookViewCommand.RaiseCanExecuteChanged();
    }

}
