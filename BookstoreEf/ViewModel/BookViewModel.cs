using BookstoreEf.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreEf.ViewModel
{
    class BookViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public DelegateCommand AddBookCommand { get; }

        public BookViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddBookCommand = new DelegateCommand(AddBook);

        }

        private void AddBook(object obj)
        {
            throw new NotImplementedException();
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
                UpdateBookISBN();
            }
        }

        private void UpdateBookISBN()
        {
            using var db = new BookstoreContext();
            var book = db.Books.FirstOrDefault(b => b.Isbn == SelectedBook.Isbn);
            if (book != null)
            {
                book.Isbn = _isbn;
                db.SaveChanges();
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
                UpdateBookTitle();
            }
        }

        private string _authorFullName;

        public string AuthorFullName
        {
            get { return _authorFullName; }
            set { _authorFullName = value; }
        }

        private void UpdateBookTitle()
        {
            using var db = new BookstoreContext();
            var book = db.Books.FirstOrDefault(b => b.Isbn == SelectedBook.Isbn);
            if (book != null)
            {
                Title = book.BookTitle;
            }
            else
            {
                Title = "Book not found";
            }
        }

    }
}
