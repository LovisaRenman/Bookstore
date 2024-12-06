using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreEf.ViewModel
{
    internal class BookInventoryViewModel : ViewModelBase
    {
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

        public decimal TotalPrice => Quantity * Price;
    }
}
