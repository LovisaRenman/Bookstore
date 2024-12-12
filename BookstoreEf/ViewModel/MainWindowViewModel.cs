using BookstoreEf.Command;

namespace BookstoreEf.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{
    public AuthorViewModel AuthorViewModel { get; }
    public BookViewModel BookViewModel { get; }
    public StoreInventoryViewModel StoreInventoryViewModel { get; }


    private bool _isFullscreen;
    public bool IsFullScreen
    {
        get => _isFullscreen;
        set
        {
            _isFullscreen = value;
            RaisePropertyChanged();
        }
    }


    public event EventHandler ExitProgramRequested;
    public event EventHandler<bool> ToggleFullscreenRequested;
    
    public DelegateCommand ExitProgramCommand { get; }
    public DelegateCommand ToggleFullscreenCommand { get; }

    public MainWindowViewModel()
    {
        AuthorViewModel = new AuthorViewModel(this);
        BookViewModel = new BookViewModel(this);
        StoreInventoryViewModel = new StoreInventoryViewModel(this);

        IsFullScreen = false;

        ExitProgramCommand = new DelegateCommand(ExitProgram);
        ToggleFullscreenCommand = new DelegateCommand(ToggleFullscreen);
    }

    private void ExitProgram(object obj) => ExitProgramRequested.Invoke(this, EventArgs.Empty);

    private void ToggleFullscreen(object obj) 
    {
        IsFullScreen = !IsFullScreen;
        ToggleFullscreenRequested.Invoke(this, IsFullScreen); 
    }

}
