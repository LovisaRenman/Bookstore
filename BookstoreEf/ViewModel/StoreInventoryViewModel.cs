using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel;

class StoreInventoryViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;


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

        _selectedStore = Stores?.FirstOrDefault();
    }

    public void LoadStores() 
    {
        using var db = new BookstoreContext();
        var stores = db.Stores.ToList();

        Stores = new ObservableCollection<Store>(stores);
    }

    public void GetStoreAdress()
    {
        foreach (var store in Stores)
        {
            store.StoreAdress = string.Join(", " , string.Join(" ", store.Street, store.StreetNumber), store.Postcode, store.City, store.Country);
        }
    }


}
