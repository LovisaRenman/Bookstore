﻿using BookstoreEf.Dialogs;
using BookstoreEf.ViewModel;
using System.Diagnostics.Eventing.Reader;
using System.Windows;

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
        mainWindowViewModel.BookViewModel.CloseBookDialog += OnCloseDialogRequested;
        mainWindowViewModel.BookViewModel.ShowDialogAddBooks += AddBooks;
        mainWindowViewModel.BookViewModel.ShowDialogEditBook += EditBooks;
        mainWindowViewModel.BookViewModel.ShowMessageBoxRemoveBook += DeleteBook;
        mainWindowViewModel.StoreInventoryViewModel.CloseManageInventoryDialog += OnCloseDialogRequested;
        mainWindowViewModel.StoreInventoryViewModel.OpeInventoryDialog += OnOpenInventoryRequested;
    }

    public void AddBooks(object? sender, EventArgs arg)
    {
        _dialog = new AddBook();
        ShowDialog(mainWindowViewModel.BookViewModel);
    }

    public void DeleteBook(object? sender, EventArgs arg)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this Book", "Remove Book", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes) mainWindowViewModel.BookViewModel.Books.Remove(mainWindowViewModel.BookViewModel.SelectedBook);
    }

    private void EditBooks(object? sender, EventArgs e)
    {
        _dialog = new AddBook();
        ShowDialog(mainWindowViewModel.BookViewModel);
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
        MessageBoxResult result = MessageBox.Show("All books associated with this author will be permanently removed from the system. Do you want to continue?",
            "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            mainWindowViewModel.AuthorViewModel.Authors.Remove(author);
        }
    }

    private void OnExitProgramRequested(object? sender, EventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to exit \"Once Upon a Bookstore\"?", "Exit bookstore",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
    }

    private void OnOpenInventoryRequested(object? sender, EventArgs e)
    {
        _dialog = new ManageInventory();
        ShowDialog(mainWindowViewModel.StoreInventoryViewModel);
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