using BookstoreEf.Dialogs;
using BookstoreEf.ViewModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BookstoreEf;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel? mainWindowViewModel;
    private Window _dialog;

    public MainWindow()
    {
        InitializeComponent();
        mainWindowViewModel = new MainWindowViewModel();
        DataContext = mainWindowViewModel;

        mainWindowViewModel.BookViewModel.ShowDialogAddBooks += AddBooks;
        mainWindowViewModel.BookViewModel.ShowDialogEditBook += EditBooks;
        mainWindowViewModel.BookViewModel.ShowMessageBoxRemoveBook += RemoveBook;
    }

    private void EditBooks(object? sender, EventArgs e)
    {
        _dialog = new AddBook();
        ShowDialog(mainWindowViewModel.BookViewModel);
    }

    public void AddBooks(object? sender, EventArgs arg)
    {
        _dialog = new AddBook();
        ShowDialog(mainWindowViewModel.BookViewModel);
    }

    public void RemoveAuthor(object? sender, EventArgs arg)
    {
        MessageBox.Show("Are you sure you want to remove this author", "Remove Author", MessageBoxButton.YesNo, MessageBoxImage.Question);
    }

    public void RemoveAuthorWarning(object? sender, EventArgs arg)
    {
        MessageBox.Show("All books associated with this author will be removed, do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
    }

    public void RemoveBook(object? sender, EventArgs arg)
    {
        MessageBox.Show("Are you sure you want to remove this Book", "Remove Book", MessageBoxButton.YesNo, MessageBoxImage.Question);
    }
    public void ShowDialog(object viewModel)
    {
        try
        {
            _dialog.DataContext = viewModel;
            _dialog.Owner = Application.Current.MainWindow;
            _dialog.Show();
        }
        catch (Exception e)
        {
            MessageBox.Show($"An error occurred while opening the dialog box {e.Message}");
        }
    }
}