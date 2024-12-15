using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreEf.ViewModel
{
    internal class BookInventoryTranslate : ViewModelBase
    {
        private string _bookisbn;
        public string BookIsbn
        {
            get => _bookisbn;
            set
            {
                _bookisbn = value;
                RaisePropertyChanged();
            }
        }

        private string _bookTitle;

        public string BookTitle
        {
            get => _bookTitle;
            set
            {
                _bookTitle = value;
                RaisePropertyChanged();
            }
        }


        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                RaisePropertyChanged();
                RaisePropertyChanged("TotalPrice");
            }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                RaisePropertyChanged();
                RaisePropertyChanged("TotalPrice");
            }
        }

        [NotMapped]
        public decimal TotalPrice => Quantity * Price;
    }
}
