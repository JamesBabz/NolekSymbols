using System.ComponentModel;
using NolekSymbols.Model;
using NolekSymbols.View;

namespace NolekSymbols.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public MainViewModel()
        {
            SymbolListViewModel = new SymbolListViewModel(this);
            SymbolViewModel = new SymbolViewModel(this);
            SymbolTabViewModel = new SymbolTabViewModel(this);
            LineViewModel = new LineViewModel(this);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Opens the 'save changes' window as a popup
        /// </summary>
        public void CreatePopUp()
        {
            var pop = new SaveChangesWindow(this);
            pop.ShowDialog();
        }

        #endregion Methods

        #region Properties

        public SymbolListViewModel SymbolListViewModel { get; }
        public SymbolViewModel SymbolViewModel { get; }
        public SymbolTabViewModel SymbolTabViewModel { get; }
        public LineViewModel LineViewModel { get; }
        public SymbolModel _selectedSymbol { get; set; }

        public SymbolModel SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                _selectedSymbol = value;
                SymbolTabViewModel.SelectedSymbol = value;
            }
        }

        #endregion Properties

        #region INotify

        /// <inheritdoc />
        /// <summary>
        ///     PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Notify property changed
        /// </summary>
        /// <param name="prop">property name</param>
        public void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}