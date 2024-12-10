using BookstoreEf.Command;
using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BookstoreEf.ViewModel
{
    class BookViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public DelegateCommand AddBookCommand { get; }
        public DelegateCommand EditBookCommand { get; }
        public DelegateCommand RemoveBookCommand { get; }
        public DelegateCommand CreateCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public event EventHandler ShowDialogAddBooks;
        public event EventHandler ShowDialogEditBook;
        public event EventHandler ShowMessageBoxRemoveBook;
        public event EventHandler CloseBookDialog;


        public BookViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddBookCommand = new DelegateCommand(AddBook);
            EditBookCommand = new DelegateCommand(EditBook, EditBookActive);
            RemoveBookCommand = new DelegateCommand(RemoveBook, RemoveBookActive);
            CreateCommand = new DelegateCommand(Create);
            CancelCommand = new DelegateCommand(Cancel);

            LoadBooks();
        }

        private void Cancel(object obj)
        {
            CloseBookDialog.Invoke(this, EventArgs.Empty);
        }

        private void Create(object obj)
        {
            //SelectedBook.Authors = SelectedAuthor;
            SelectedBook.Genre = SelectedGenre;
            SelectedBook.Publisher = SelectedPublisher;

            CloseBookDialog.Invoke(this, EventArgs.Empty);
        }

        private bool RemoveBookActive(object? arg)
        {
            if (SelectedBook != null) return true;
            else return false;
        }

        private void RemoveBook(object obj)
        {
            ShowMessageBoxRemoveBook.Invoke(this, EventArgs.Empty);
        }

        private bool EditBookActive(object? arg)
        {
            if (SelectedBook != null) return true;
            else return false;
        }

        private void EditBook(object obj)
        {
            BookWindowTitle = "Edit book";
            ShowDialogEditBook.Invoke(this, EventArgs.Empty);

            //SelectedAuthor = SelectedBook.Authors;
            SelectedGenre = SelectedBook.Genre;
            SelectedPublisher = SelectedBook.Publisher;
        }

        private void AddBook(object obj)
        {
            using var db = new BookstoreContext();

            ShowDialogAddBooks.Invoke(this, EventArgs.Empty);
            var newBook = new Book() { Isbn = string.Empty, BookTitle = string.Empty, Price = 0, PublishDate = DateOnly.MinValue, Pages = 0 };
            Books.Add(newBook);
            SelectedBook = newBook;

            //SelectedAuthor = Authors.FirstOrDefault();
            SelectedGenre = Genres.FirstOrDefault();
            SelectedPublisher = Publishers.FirstOrDefault();

            BookWindowTitle = "Add book";
        }
        
        public void LoadBooks()
        {
            using var db = new BookstoreContext();
            var books = db.Books.Include(b => b.Genre).Include(b => b.Publisher).Include(b => b.Authors).ToList();
            Books = new ObservableCollection<Book>(books);
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


        public List<Author> _authors { get; set; }
        public List<Author> Authors
        {
            get
            {
                using var db = new BookstoreContext();
                _authors = db.Authors.OrderBy(a => a.LastName).ToList();                
                return _authors;
            }
        }
        public Author _selectedAuthor { get; set; }
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                RaisePropertyChanged();
            }
        }


        public List<Genre> _genres { get; set; }
        public List<Genre> Genres
        {
            get
            {
                using var db = new BookstoreContext();
                _genres = db.Genres.ToList();
                return _genres;
            }
        }
        public Genre _selectedGenre { get; set; }
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                RaisePropertyChanged();
            }
        }


        public List<Publisher> _publishers { get; set; }
        public List<Publisher> Publishers
        {
            get
            {
                using var db = new BookstoreContext();
                _publishers = db.Publishers.ToList();
                return _publishers;
            }
        }
        public Publisher _selectedPublishers { get; set; }
        public Publisher SelectedPublisher
        {
            get => _selectedPublishers;
            set
            {
                _selectedPublishers = value;
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
    }
}
