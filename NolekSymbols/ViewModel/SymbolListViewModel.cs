using System;
using System.Collections.ObjectModel;
using NolekSymbols.Helpers;
using NolekSymbols.Model;

namespace NolekSymbols.ViewModel
{
    public class SymbolListViewModel
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mainViewModel"></param>
        public SymbolListViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Symbols = new ObservableCollection<SymbolModel>();
            foreach (var t in AbstractClassHelper.GetAllTypesInNameSpace("NolekSymbols.Model.BE"))
                Symbols.Add((SymbolModel) Activator.CreateInstance(t));
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