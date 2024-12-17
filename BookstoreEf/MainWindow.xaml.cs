using BookstoreEf.Dialogs;
using BookstoreEf.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TextBox = System.Windows.Controls.TextBox;

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

        mainWindowViewModel.ExitProgramRequested += OnExitProgramRequested;
        mainWindowViewModel.ToggleFullscreenRequested += OnToggleFullscreenRequested;
        mainWindowViewModel.AuthorViewModel.DeleteAuthorRequested += OnDeleteAuthorRequested;
        mainWindowViewModel.AuthorViewModel.FailedToAddAuthor += OnFailedToAddAuthor;
        mainWindowViewModel.AuthorViewModel.FailedToSaveAuthor += OnFailedToSaveAuthor;
        mainWindowViewModel.AuthorViewModel.SuccessfullySaveAuthor += OnSuccessfullySaveAuthor;
        mainWindowViewModel.BookViewModel.CloseBookDialog += OnCloseDialogRequested;
        mainWindowViewModel.BookViewModel.FailedBookUpdate += FailedBookUpdate;
        mainWindowViewModel.BookViewModel.ShowDialogAddBooks += AddBooks;
        mainWindowViewModel.BookViewModel.ShowDialogEditBook += EditBooks;
        mainWindowViewModel.BookViewModel.ShowMessageBoxRemoveBook += DeleteBook;
        mainWindowViewModel.BookViewModel.UpdateSource += UpdateSourceAddBook;
        mainWindowViewModel.StoreInventoryViewModel.CloseAddBookToSelectedStoreDialog += OnCloseDialogRequested;
        mainWindowViewModel.StoreInventoryViewModel.CloseManageInventoryDialog += OnCloseDialogRequested;
        mainWindowViewModel.StoreInventoryViewModel.OpenAddBookToStoreDialog += OnOpenAddBooktitleToStoreRequested;
        mainWindowViewModel.StoreInventoryViewModel.DeleteBookFromStoreRequested += OnDeleteBookFromStoreRequested;
        mainWindowViewModel.StoreInventoryViewModel.OpenInventoryDialog += OnOpenInventoryRequested;
        mainWindowViewModel.StoreInventoryViewModel.UpdateSliderQuantity += OnUpdateSourceManageInventory;
        mainWindowViewModel.StoreInventoryViewModel.FailedToUpdateQuantity += OnFailedToUpdateQuantity;
        mainWindowViewModel.StoreInventoryViewModel.NoBooksToAddMessage += OnNoBooksToAddMessage;
        mainWindowViewModel.StoreInventoryViewModel.ChooseABookMessage += OnChooseABookMessage;
    }

    private void OnChooseABookMessage(object? sender, EventArgs e)
    {
        System.Windows.MessageBox.Show("Select a book title from the options provided.",
            "Select a booktitle", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private void OnNoBooksToAddMessage(object? sender, EventArgs e)
    {
        System.Windows.MessageBox.Show("There are no books available for adding to the store at this time. Please try again soon.",
            "No booktitle avaiable", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void AddBooks(object? sender, EventArgs arg)
    {
        _dialog = new AddBook();
        ShowDialog(mainWindowViewModel.BookViewModel);        
    }

    public void DeleteBook(object? sender, Book book)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to remove this Book", "Remove Book", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            mainWindowViewModel.BookViewModel.Books.Remove(book);
            using var db = new BookstoreContext();
            db.Books.Remove(book);
            try
            {
                db.SaveChanges();
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show($"You were unable to Update the Databse: {e.Message}", "Unsuccessful update", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            mainWindowViewModel.BookViewModel.LoadBooks();
        }
    }

    private void EditBooks(object? sender, EventArgs e)
    {
        _dialog = new AddBook();
        ShowDialog(mainWindowViewModel.BookViewModel);
    }

    private void FailedBookUpdate(object? sender, Exception e)
    {
        System.Windows.MessageBox.Show($"You were unable to Update the Databse: {e.Message}", "Unsuccessful update", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void OnCloseDialogRequested(object? sender, EventArgs args)
    {
        if (_dialog != null)
        {
            _dialog?.Close();
            _dialog = null;
        }
    }

    public void OnDeleteAuthorRequested(object? sender, Author author)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show("All books associated with this author will be permanently removed from the system. Do you want to continue?",
            "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            using var db = new BookstoreContext();
            db.Authors.Remove(author);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Failed to delete author: {e.Message}.");
            }
        }
    }

    private void OnDeleteBookFromStoreRequested(object? sender, BookInventoryTranslate selectedBook)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to delete selected booktitle from the store? ", "Delete selected booktitle?",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            mainWindowViewModel.StoreInventoryViewModel.BooksInSelectedStore.Remove(selectedBook);
            using var db = new BookstoreContext();
            var inventory = db.Inventories.FirstOrDefault(i => i.StoreId == mainWindowViewModel.StoreInventoryViewModel.SelectedStore.Id && i.BookIsbn == selectedBook.BookIsbn);
            db.Inventories.Remove(inventory);
            db.SaveChanges();
        }
    }

    private void OnExitProgramRequested(object? sender, EventArgs e)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to exit \"Once Upon a Bookstore\"?", "Exit bookstore",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

    private void OnFailedToAddAuthor(object? sender, Exception message)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show($"Failed to add author: {message}.",
            "Warning", MessageBoxButton.OK);
    }

    private void OnFailedToSaveAuthor(object? sender, Exception message)
    {
        System.Windows.MessageBox.Show($"Failed to save the author's information. Error message: {message}", 
            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void OnFailedToUpdateQuantity(object? sender, EventArgs e)
    {
        System.Windows.MessageBox.Show("Error: book quantity could not be updated");
    }

    private void OnOpenAddBooktitleToStoreRequested(object? sender, EventArgs e)
    {
        _dialog = new AddBookToSelectedStore();
        ShowDialog(mainWindowViewModel.StoreInventoryViewModel);
    }

    private void OnOpenInventoryRequested(object? sender, EventArgs e)
    {
        _dialog = new ManageInventory();
        ShowDialog(mainWindowViewModel.StoreInventoryViewModel);
    }

    private void OnSuccessfullySaveAuthor(object? sender, EventArgs e)
    {
        System.Windows.MessageBox.Show("All changes for the author have been saved.", 
            "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OnToggleFullscreenRequested(object? sender, bool isFullscreen)
    {
        if (isFullscreen)
        {
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
        }
        else
        {
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }
    }

    private void OnUpdateSourceManageInventory(object? sender, EventArgs e)
    {
        if (_dialog is ManageInventory dialog)
        {         
            BindingExpression sliderBinding = dialog.slider.GetBindingExpression(Slider.ValueProperty);
            sliderBinding?.UpdateSource();
        }
    }

    public void ShowDialog(object viewModel)
    {
        try
        {
            _dialog.DataContext = viewModel;
            _dialog.Owner = System.Windows.Application.Current.MainWindow;
            _dialog.Show();
        }
        catch (Exception e)
        {
            System.Windows.MessageBox.Show($"An error occurred while opening the dialog box {e.Message}");
        }
    }

    private void UpdateSourceAddBook(object? sender, EventArgs e)
    {        
        if (_dialog is AddBook addBookDialog)
        {
            BindingExpression beISBN = addBookDialog.ISBNTb.GetBindingExpression(TextBox.TextProperty);
            beISBN.UpdateSource();

            BindingExpression beTitle = addBookDialog.TitleTb.GetBindingExpression(TextBox.TextProperty);
            beTitle.UpdateSource();

            BindingExpression bePublishDate = addBookDialog.PublishDateTb.GetBindingExpression(DatePicker.TextProperty);
            bePublishDate.UpdateSource();

            BindingExpression beLanguage = addBookDialog.LanguageTb.GetBindingExpression(TextBox.TextProperty);
            beLanguage.UpdateSource();

            BindingExpression bePrice = addBookDialog.PriceTb.GetBindingExpression(TextBox.TextProperty);
            bePrice.UpdateSource();

            BindingExpression bePages = addBookDialog.PagesTb.GetBindingExpression(TextBox.TextProperty);
            bePages.UpdateSource();

            BindingExpression beWeight = addBookDialog.WeightTb.GetBindingExpression(TextBox.TextProperty);
            beWeight.UpdateSource();
        }
    }
}