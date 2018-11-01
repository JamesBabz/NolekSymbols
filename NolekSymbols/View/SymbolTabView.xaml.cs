using System.Windows;
using System.Windows.Controls;
using NolekSymbols.ViewModel;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for SymbolTabView.xaml
    /// </summary>
    public partial class SymbolTabView
    {
        #region Properties

        private SymbolTabViewModel _dataContext;

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public SymbolTabView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Set datacontext on load
        /// </summary>
        private void MainLoaded(object sender, RoutedEventArgs e)
        {
            _dataContext = DataContext as SymbolTabViewModel;
        }

        /// <summary>
        ///     If the value of the name is different, save the new value to the list of unsaved changes
        /// </summary>
        /// <param name="sender">The textbox</param>
        /// <param name="routedEventArgs"></param>
        private void NameChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!(sender is TextBox tb)
                || _dataContext.SelectedSymbol == null
                || tb.Text.Equals(_dataContext.SelectedSymbol.Name)) return;
            var be = tb.GetBindingExpression(TextBox.TextProperty);
            if (be != null) _dataContext.UnsavedChanges.Add(be);
        }

        /// <summary>
        ///     If the value of the Tooltip is different, save the new value to the list of unsaved changes
        /// </summary>
        /// <param name="sender">The textbox</param>
        /// <param name="e"></param>
        private void TooltipChanged(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox tb)
                || _dataContext.SelectedSymbol == null
                || tb.Text.Equals(_dataContext.SelectedSymbol.Tooltip)) return;
            var be = tb.GetBindingExpression(TextBox.TextProperty);
            if (be != null) _dataContext.UnsavedChanges.Add(be);
        }

        /// <summary>
        ///     Save button click event
        /// </summary>
        private void SaveButton_OnCLick(object sender, RoutedEventArgs e)
        {
            _dataContext.SaveAll();
        }

        /// <summary>
        ///     Cancel button click event
        /// </summary>
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            _dataContext.DiscardChanges();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            _dataContext.MainViewModel.SymbolViewModel.Symbols.Remove(_dataContext.SelectedSymbol);
            _dataContext.SelectedSymbol = null;
        }

        #endregion Methods
    }
}