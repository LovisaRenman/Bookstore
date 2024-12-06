using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreEf.ViewModel;

class StoreInventoryViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;


    private bool _textVisibility;
    public bool TextVisibility
    {
        get => _textVisibility;
        set
        {
            _textVisibility = value;
            RaisePropertyChanged();
        }
    }


    public StoreInventoryViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        //DeleteQuestionIsEnable = false;
        //IsConfigurationModeVisible = true;


        // Delegat commands här ...

        //SelectedStore = ?????????????????.FirstOrDefault();
        //TextVisibility = ActivePack?.Questions.Count > 0;


    }



    //private void ChangeTextVisibility()
    //        => TextVisibility = XX.Count > 0 && SelectedStore != null;
}
