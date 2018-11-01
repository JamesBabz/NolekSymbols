using System.Windows;
using NolekSymbols.ViewModel;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for SaveChangesWindow.xaml
    /// </summary>
    public partial class SaveChangesWindow
    {
        #region Properties

        private readonly MainViewModel _mainViewModel;

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mainViewModel">The MainViewModel</param>
        public SaveChangesWindow(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Saves the settings on the symbol
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.SymbolTabViewModel.SaveAll();
            Close();
        }

        /// <summary>
        ///     Discards the settings
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void DiscardButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.SymbolTabViewModel.DiscardChanges();
            Close();
        }

        #endregion Methods
    }
}