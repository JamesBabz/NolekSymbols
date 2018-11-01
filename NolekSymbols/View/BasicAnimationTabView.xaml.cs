using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using NolekSymbols.ViewModel;
using Xceed.Wpf.Toolkit;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for BasicAnimationTabView.xaml
    /// </summary>
    public partial class BasicAnimationTabView
    {
        #region Properties

        private SymbolTabViewModel _dataContext;

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public BasicAnimationTabView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Changes the throughput on selection change
        /// </summary>
        /// <param name="sender">the combobox</param>
        /// <param name="e">Arguments</param>
        private void DesiredThroughput_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox cb)
                || _dataContext?.SelectedSymbol == null
                || e.RemovedItems.Count == 0) return;
            var be = cb.GetBindingExpression(Selector.SelectedIndexProperty);
            if (be != null) _dataContext.UnsavedChanges.Add(be);
        }

        /// <summary>
        ///     Set datacontext on load
        /// </summary>
        private void MainLoaded(object sender, RoutedEventArgs e)
        {
            _dataContext = DataContext as SymbolTabViewModel;
        }

        /// <summary>
        ///     If the value of the color picker is different, save the new value to the list of unsaved changes
        /// </summary>
        /// <param name="sender">The color picker</param>
        /// <param name="routedPropertyChangedEventArgs">Color arguments</param>
        private void ColorPickerChanged(object sender,
            RoutedPropertyChangedEventArgs<Color?> routedPropertyChangedEventArgs)
        {
            if (routedPropertyChangedEventArgs.OldValue.ToString().Equals("")) return;
            if (!(sender is ColorPicker cp)
                || _dataContext.SelectedSymbol == null) return;
            var be = cp.GetBindingExpression(ColorPicker.SelectedColorProperty);
            if (be != null) _dataContext.UnsavedChanges.Add(be);
        }

        #endregion Methods
    }
}