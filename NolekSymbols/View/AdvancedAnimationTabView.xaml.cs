using System.Windows;
using System.Windows.Media;
using NolekSymbols.Model;
using NolekSymbols.ViewModel;
using Xceed.Wpf.Toolkit;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for AdvancedAnimationTabView.xaml
    /// </summary>
    public partial class AdvancedAnimationTabView
    {
        #region Properties

        private SymbolTabViewModel _dataContext;

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public AdvancedAnimationTabView()
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


        /// <summary>
        ///     Add condition button click event
        /// </summary>
        private void AddColorCondition_OnCLick(object sender, RoutedEventArgs e)
        {
            _dataContext.SelectedSymbol.Image.AddColorCondition(new ColorDetermineModel("=", Colors.Gray));
        }

        /// <summary>
        ///     Removes the colorcondition from the symbol
        /// </summary>
        /// <param name="sender">The view</param>
        /// <param name="e">Arguments</param>
        private void RemoveColorCondition_OnClick(object sender, RoutedEventArgs e)
        {
            var cdm = ((FrameworkElement) sender).DataContext as ColorDetermineModel;
            _dataContext.SelectedSymbol.Image.RemoveColorCondition(cdm);
        }

        #endregion Methods
    }
}