using BookstoreEf.ViewModel;
using System.Windows;

namespace BookstoreEf;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel? mainWindowViewModel;

    public MainWindow()
    {
        InitializeComponent();
        mainWindowViewModel = new MainWindowViewModel();
        DataContext = mainWindowViewModel;
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
}