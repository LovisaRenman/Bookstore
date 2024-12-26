using BookstoreEf.Command;
using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel
{
    class BookViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                RaisePropertyChanged();
            }
        }

        private Book? _savedSelectedBook;
        public Book? NewBook
        {
            get => _savedSelectedBook;
            set
            {
                _savedSelectedBook = value;
            }
        }

        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                RaisePropertyChanged();
                EditBookCommand.RaiseCanExecuteChanged();
                RemoveBookCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isBookViewMenuOptionEnable;
        public bool IsBookViewMenuOptionEnable
        {
            get => _isBookViewMenuOptionEnable; 
            set 
            { 
                _isBookViewMenuOptionEnable = value;
                RaisePropertyChanged();
            }
        }

        private bool _isBookViewVisible;
        public bool IsBookViewVisible
        {
            get => _isBookViewVisible; 
            set 
            { 
                _isBookViewVisible = value;
                RaisePropertyChanged();
            }
        }

        private DateOnly _publishDate;
        public DateOnly PublishDate
        {
            get => _publishDate;
            set
            {
                _publishDate = value;
                RaisePropertyChanged();
            }
        }

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                RaisePropertyChanged();
            }
        }

        private List<Author> _authors; 
        public List<Author> Authors
        {
            get =>_authors;
            set 
            {
                _authors = value;
                RaisePropertyChanged();
            }
        }

        private List<Genre> _genres;
        public List<Genre> Genres
        {
            get => _genres;
            set 
            {
                _genres = value;
                RaisePropertyChanged();
            }
        }

        private List<Publisher> _publishers;
        public List<Publisher> Publishers
        {
            get => _publishers;
            set 
            {
                _publishers = value;
                RaisePropertyChanged();
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

        private Publisher _selectedPublishers;
        public Publisher SelectedPublisher
        {
            get => _selectedPublishers;
            set
            {
                _selectedPublishers = value;
                RaisePropertyChanged();
            }
        }

        private string _bookWindowTitle;
        public string BookWindowTitle
        {
            get => _bookWindowTitle;
            set
            {
                _bookWindowTitle = value;
                RaisePropertyChanged();
            }
        }

        private string _buttonContent;
        public string ButtonContent
        {
            get => _buttonContent; 
            set 
            {
                _buttonContent = value;
                RaisePropertyChanged();
            }
        }

        public string TitleAddBook = "Add Book";
        public string TitleEditBook = "Edit Book";


        public event EventHandler CloseBookDialog;
        public event EventHandler ShowDialogAddBooks;
        public event EventHandler ShowDialogEditBook;
        public event EventHandler<Book> ShowMessageBoxRemoveBook;
        public event EventHandler UpdateSource;
        public event EventHandler<Exception> FailedBookUpdate;

        public DelegateCommand AddBookCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand CreateCommand { get; }
        public DelegateCommand EditBookCommand { get; }
        public DelegateCommand RemoveBookCommand { get; }
        public DelegateCommand SwitchToBookViewCommand { get; }

        public BookViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            IsBookViewMenuOptionEnable = true;

            LoadBooks();
            
            AddBookCommand = new DelegateCommand(AddBook);
            EditBookCommand = new DelegateCommand(EditBook, EditBookActive);
            RemoveBookCommand = new DelegateCommand(RemoveBook, RemoveBookActive);
            CreateCommand = new DelegateCommand(CreateOrChange);
            CancelCommand = new DelegateCommand(Cancel);
            SwitchToBookViewCommand = new DelegateCommand(StartBookView, IsBookViewEnable);
        }

        public void LoadBooks()
        {
            using var db = new BookstoreContext();
            var books = db.Books.Include(b => b.Genre).Include(b => b.Publisher).Include(b => b.Author).ToList();
            Books = new ObservableCollection<Book>(books);
        }

        private void AddBook(object obj)
        {
            BookWindowTitle = TitleAddBook;
            ButtonContent = "Create";

            ShowDialogAddBooks.Invoke(this, EventArgs.Empty);
            NewBook = new Book() { Isbn = string.Empty, BookTitle = string.Empty, Price = 0, PublishDate = DateOnly.MinValue, Pages = 0 };
            //Books.Add(NewBook);
            SelectedBook = NewBook;
            LoadCombobox();
        }

        private void Cancel(object obj)
        {
            if (BookWindowTitle == TitleAddBook)
            {
                Books.Remove(NewBook);
            }
            CloseBookDialog.Invoke(this, EventArgs.Empty);
        }            

        private void CreateOrChange(object obj)
        {
            using var db = new BookstoreContext();

            if (BookWindowTitle == TitleAddBook)
            {
                SelectedBook.AuthorId = SelectedAuthor.Id;
                SelectedBook.GenreId = SelectedGenre.Id;
                SelectedBook.PublisherId = SelectedPublisher.Id;
                UpdateSource.Invoke(this, EventArgs.Empty);

                db.Books.Add(SelectedBook);

                try 
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    FailedBookUpdate.Invoke(this, e);
                    Books.Remove(NewBook);
                }
            }
            else if (BookWindowTitle == TitleEditBook)
            {
                var bookToBeChanged = new Book(SelectedBook);

                SelectedBook.AuthorId = SelectedAuthor.Id;
                SelectedBook.GenreId = SelectedGenre.Id;
                SelectedBook.PublisherId = SelectedPublisher.Id;

                UpdateSource.Invoke(this, EventArgs.Empty);

                var bookEntry = db.Entry(db.Books.Find(bookToBeChanged.Isbn));
                bookEntry.CurrentValues.SetValues(SelectedBook);

                try
                {
                    db.SaveChanges();
                }
                catch(Exception e)
                {
                    FailedBookUpdate.Invoke(this, e);
                }
            }            

            LoadBooks();

            CloseBookDialog.Invoke(this, EventArgs.Empty);
        }

        private bool EditBookActive(object? arg)
        {
            if (SelectedBook != null) return true;
            else return false;
        }

        private void EditBook(object obj)
        {
            BookWindowTitle = TitleEditBook;
            ButtonContent = "Change";
            ShowDialogEditBook.Invoke(this, EventArgs.Empty);

            LoadCombobox();
        }

        public void LoadCombobox()
        {
            using var db = new BookstoreContext();
            int i = 0;

            if (db.Authors != null)
            {
                foreach (var author in db.Authors)
                {
                    i = author.Id;
                    break;
                }
            } 
            
            SelectedAuthor = db.Authors.FirstOrDefault(a => a.Id == SelectedBook.AuthorId || a.Id == i);
            Authors = db.Authors.ToList();

            if (db.Genres != null)
            {
                foreach (var genre in db.Genres)
                {
                    i = genre.Id;
                    break;
                }
            }

            SelectedGenre = db.Genres.FirstOrDefault(g => g.Id == SelectedBook.GenreId || g.Id == 1);
            Genres = db.Genres.ToList();

            if (db.Publishers != null)
            {
                foreach (var publisher in db.Publishers)
                {
                    i = publisher.Id;
                    break;
                }
            }

            SelectedPublisher = db.Publishers.FirstOrDefault(p => p.Id == SelectedBook.PublisherId || p.Id == 1);
            Publishers = db.Publishers.ToList();
        }

        private bool RemoveBookActive(object? arg)
        {
            if (SelectedBook != null) return true;
            else return false;
        }

        private void RemoveBook(object obj) => ShowMessageBoxRemoveBook.Invoke(this, SelectedBook);

        public void StartBookView(object? obj)
        {
            IsBookViewVisible = true;
            mainWindowViewModel.AuthorViewModel.IsAuthorViewVisible = false;
            mainWindowViewModel.StoreInventoryViewModel.IsStoreInventoryViewVisible = false;

            UpdateCommandStates();
        }

        private bool IsBookViewEnable(object? obj) => IsBookViewMenuOptionEnable = !IsBookViewVisible;

        private void UpdateCommandStates()
        {
            SwitchToBookViewCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.AuthorViewModel.SwitchToAuthorViewCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.StoreInventoryViewModel.SwitchToStoreInventoryViewCommand.RaiseCanExecuteChanged();
        }

    }
}
