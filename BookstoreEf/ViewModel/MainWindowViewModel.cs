using BookstoreEf.Command;

namespace BookstoreEf.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{
    public AuthorViewModel AuthorViewModel { get; }
    public BookViewModel BookViewModel { get; }
    public StoreInventoryViewModel StoreInventoryViewModel { get; }

    public MainWindowViewModel()
    {
        AuthorViewModel = new AuthorViewModel(this);
        BookViewModel = new BookViewModel(this);
        StoreInventoryViewModel = new StoreInventoryViewModel(this);

    }
}
