using BookstoreEf.Command;
using BookstoreEf.Model;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel;

class StoreInventoryViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;


    private BookInventoryViewModel _selectedBookTitle;
    public BookInventoryViewModel SelectedBookTitle
    {
        get => _selectedBookTitle; 
        set 
        { 
            _selectedBookTitle = value;
            RaisePropertyChanged();
        }
    }
   
    private ObservableCollection<BookInventoryViewModel> _booksInSelectedStore;
    public ObservableCollection<BookInventoryViewModel> BooksInSelectedStore
    {
        get => _booksInSelectedStore;
        set
        {
            _booksInSelectedStore = value;
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


    private string _storeNames;
    public string StoreNames
    {
        get => _storeNames; 
        set
        {
            if (_storeNames != value)
            {
                _storeNames = value;
                RaisePropertyChanged();
            }
        }
    }


    public event EventHandler OpenManageInventoryDialog;
    public event EventHandler CloseManageInventoryDialog;

    public DelegateCommand CloseManageInventoryCommand { get; }
    public DelegateCommand OpenManageInventoryCommand { get; }
    public DelegateCommand SwitchToStoreInventoryViewCommand { get; }


    public StoreInventoryViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
        
        IsStoreInventoryViewVisible = true;

        LoadStores();
        GetStoreAdress();

        CloseManageInventoryCommand = new DelegateCommand(CloseInventory);
        OpenManageInventoryCommand = new DelegateCommand(OpenInventory);
        SwitchToStoreInventoryViewCommand = new DelegateCommand(StartInventoryView, IsInventoryViewEnable);

        SelectedStore = Stores?.FirstOrDefault();
    } 

    private void LoadStores() 
    {
        using var db = new BookstoreContext();
        var stores = db.Stores.ToList();

        Stores = new ObservableCollection<Store>(stores);
    }

    private void GetStoreAdress()
    {
        foreach (var store in Stores)
        {
            store.StoreAdress = string.Join(", " , string.Join(" ", store.Street, store.StreetNumber), store.Postcode, store.City, store.Country);
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

    private bool IsInventoryViewEnable(object? obj) => IsStoreInventoryMenuOptionEnable = !IsStoreInventoryViewVisible;

    private void OpenInventory(object? obj) => OpenManageInventoryDialog.Invoke(this, EventArgs.Empty);

    private void CloseInventory(object obj)
    {
        CloseManageInventoryDialog.Invoke(this, EventArgs.Empty);
    }

    private void UpdateBooksForSelectedStore()
    {
        if (SelectedStore == null)
        {
            BooksInSelectedStore = new ObservableCollection<BookInventoryViewModel>();
            TotalInventoryValue = 0;            
            return;
        }

        using (var db = new BookstoreContext())
        {
            var booksInStore = db.Inventories
                .Where(i => i.StoreId == SelectedStore.Id)
                .Select(i => new BookInventoryViewModel
                {
                    BookTitle = i.BookIsbnNavigation.BookTitle,
                    Quantity = i.Quantity,
                    Price = i.BookIsbnNavigation.Price
                })
                .ToList();

            BooksInSelectedStore = new ObservableCollection<BookInventoryViewModel>(booksInStore);
        }

        SelectedBookTitle = BooksInSelectedStore.FirstOrDefault();

        TotalInventoryValue = BooksInSelectedStore.Sum(book => book.TotalPrice);
    }

    private void UpdateCommandStates()
    {        
        SwitchToStoreInventoryViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.AuthorViewModel.SwitchToAuthorViewCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.BookViewModel.SwitchToBookViewCommand.RaiseCanExecuteChanged();
    }

}
