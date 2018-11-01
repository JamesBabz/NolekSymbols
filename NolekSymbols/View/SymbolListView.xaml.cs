using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NolekSymbols.Helpers;
using NolekSymbols.Model;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for SymbolListView.xaml
    /// </summary>
    public partial class SymbolListView
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public SymbolListView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Creates drag and drop event when mouse is down on a symbol on the list
        /// </summary>
        /// <param name="sender">The textbox containing the name of the symbol in the list</param>
        /// <param name="e">the mouse event</param>
        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is Image image)) return;
            if (!(image.DataContext is SymbolModel sm)) return;
            if (e.LeftButton != MouseButtonState.Pressed) return;
            var test = VisualTreeSearchHelper.FindChildByName<Grid>(Application.Current.MainWindow, "SymbolViewGrid");
            Console.WriteLine(test);
            test.Background = new SolidColorBrush(Colors.Transparent);
            var data = new DataObject();
            data.SetData("Object", sm);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
        }

        #endregion Methods

        private void TestButton_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(@"Test button clicked");
        }

        #region Properties

        #endregion Properties
    }
}