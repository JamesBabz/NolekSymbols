using System.Windows;
using System.Windows.Controls;
using NolekSymbols.Model;
using NolekSymbols.ViewModel;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for BasicEventTabView.xaml
    /// </summary>
    public partial class BasicEventTabView
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public BasicEventTabView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Opens or closes the valve on click
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">Arguments</param>
        private void OpenCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn)
                || !(btn.DataContext is SymbolTabViewModel stvModel)
                || stvModel.SelectedSymbol == null
                || !(stvModel.SelectedSymbol is BasicSymbolModel bsm)) return;
            bsm.IsOpen = !bsm.IsOpen;
        }

        #endregion Methods

        #region Properties

        #endregion Properties
    }
}