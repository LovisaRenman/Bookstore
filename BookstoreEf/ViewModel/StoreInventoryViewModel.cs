using BookstoreEf.Command;
using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel;

class StoreInventoryViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;

    private ObservableCollection<BookInventoryViewModel> _booksInSelectedStore;

    public event EventHandler ShowDialogManageInventory;

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
        get { return _stores; }
        set
        {
            if (_stores != value)
            {
                _stores = value;
                RaisePropertyChanged();
            }
        }
    }

    public DelegateCommand OpenInventoryCommand { get; }

    private Store _selectedStore;
    public Store SelectedStore
    {
        get { return _selectedStore; }
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

    private string _storeNames;
    public string StoreNames
    {
        get { return _storeNames; }
        set
        {
            if (_storeNames != value)
            {
                _storeNames = value;
                RaisePropertyChanged();
            }
        }
    }

    public StoreInventoryViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        LoadStores();
        GetStoreAdress();

        OpenInventoryCommand = new DelegateCommand(OpenInventory);

        _selectedStore = Stores?.FirstOrDefault();
    }

    private void OpenInventory(object obj)
    {
        ShowDialogManageInventory.Invoke(this, EventArgs.Empty);
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

    private void UpdateBooksForSelectedStore()
    {
        if (SelectedStore == null)
        {
            BooksInSelectedStore = new ObservableCollection<BookInventoryViewModel>();
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
    }

}
