using System.Collections.ObjectModel;
using NolekSymbols.Model;

namespace NolekSymbols.ViewModel
{
    public class SymbolViewModel
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mainViewModel">the main view model for future reference</param>
        public SymbolViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Symbols = new ObservableCollection<SymbolModel>();
        }

        #endregion Constructors

        #region Properties

        public MainViewModel MainViewModel { get; }

        public ObservableCollection<SymbolModel> Symbols { get; set; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}