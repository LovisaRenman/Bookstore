using BookstoreEf.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookstoreEf.ViewModel
{
    class BookViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public DelegateCommand AddBookCommand { get; }
        public DelegateCommand EditBookCommand { get; }
        public DelegateCommand RemoveBookCommand { get; }
        public DelegateCommand OkCommand { get; }
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
            OkCommand = new DelegateCommand(Ok);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel(object obj)
        {
            CloseBookDialog.Invoke(this, EventArgs.Empty);
            EmtyAllBookProperties();
        }

        private void Ok(object obj)
        {
            SelectedBook = new Book()
            {
                Isbn = ISBN,
                Language = Language,
                BookTitle = Title,
                Pages = Pages,
                Price = Price,
                PublishDate = PublishDate,
                WeightInGrams = Weight
            };
            //UpdateAllBookProperties();
            using var db = new BookstoreContext();

            db.Books.Add(SelectedBook);
            
            CloseBookDialog.Invoke(this, EventArgs.Empty);
            EmtyAllBookProperties();
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
        }

        private void AddBook(object obj)
        {
            EmtyAllBookProperties();
            BookWindowTitle = "Add book";
            ShowDialogAddBooks.Invoke(this, EventArgs.Empty);
        }

        private void EmtyAllBookProperties()
        {
            Title = string.Empty;
            ISBN = string.Empty;
            Language = string.Empty;
            PublishDate = new DateOnly(0001, 01, 01);
            Price = 0;
            Pages = 0;
            Weight = 0;
        }

        private void UpdateAllBookProperties()
        {
            using var db = new BookstoreContext();
            var book = db.Books.FirstOrDefault(b => b == SelectedBook);
            if (book != null)
            {
                book.BookTitle = Title;
                book.Isbn = ISBN;
                book.Language = Language;
                book.PublishDate = PublishDate;
                book.Price = Price;
                book.Pages = Pages;
                book.WeightInGrams = Weight;

                db.SaveChanges();
            }
        }

        private List<Book> _books;
        public List<Book> Books
        {
            get 
            {
                using var db = new BookstoreContext();
                var _books = db.Books.Include(b => b.Authors).Include(b => b.Genre).Include(b => b.Publisher).ToList();                
                return _books; 
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
                var _authors = db.Authors.OrderBy(a => a.LastName).ToList();
                foreach (var author in _authors)
                {
                    //author.Name = $"{author.FirstName} {author.LastName}";
                }
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

        private string _language;

        public string Language
        {
            get => _language; 
            set 
            {
                _language = value;
                RaisePropertyChanged();
            }
        }

        private int _price;
        public int Price
        {
            get => _price; 
            set 
            {
                _price = value;
                RaisePropertyChanged();
            }
        }

        private int _pages;
        public int Pages
        {
            get => _pages; 
            set 
            {
                _pages = value;
                RaisePropertyChanged();
            }
        }

        private int _weight;
        public int Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                RaisePropertyChanged();
            }
        }

        private string _isbn;
        public string ISBN
        {
            get => _isbn;
            set 
            {
                _isbn = value;
                RaisePropertyChanged();
            }
        }
        
        private string _title;
        public string Title
        {
            get => _title;
            set 
            {
                _title = value;
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
