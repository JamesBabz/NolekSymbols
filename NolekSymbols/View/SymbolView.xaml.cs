using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NolekSymbols.Model;
using NolekSymbols.ViewModel;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for SymbolView.xaml
    /// </summary>
    public partial class SymbolView
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public SymbolView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private Point _relativePoint;
        private Point _startCanvasPoint;
        private bool _symbolHit;
        private SymbolViewModel _symbolViewModel;

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Handles the drop part of the drag and drop event from the list
        ///     Creates a new symbol of the dropped type
        /// </summary>
        /// <param name="sender">The control being dropped</param>
        /// <param name="e">The object dragged</param>
        private void SymbolView_OnDrop(object sender, DragEventArgs e)
        {
            SymbolViewGrid.Background = null;
            var pos = e.GetPosition(SymbolViewControl); // drop position
            var obj = (SymbolModel) e.Data.GetData("Object"); // get the symbol
            if (obj == null) return;
            var symbol = (SymbolModel) Activator.CreateInstance(obj.GetType()); // Create a new instance of the object

            //Set the canvas postition to the dropped position
            symbol.CanvasTop = pos.Y;
            symbol.CanvasLeft = pos.X;
            symbol.Image?.CalculateSize();
            _symbolViewModel.Symbols.Add(symbol);
        }

        /// <summary>
        ///     Sets datacontext on load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbolView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _symbolViewModel = DataContext as SymbolViewModel;
        }

        /// <summary>
        ///     Start dragging the symbol if left control and left mouse is pressed
        /// </summary>
        /// <param name="sender">The container of the symbol</param>
        /// <param name="e">The mouse args</param>
        private void SymbolContainer_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed
                || !Keyboard.IsKeyDown(Key.LeftCtrl)
                || ((App) Application.Current).IsRunMode) return;
            var sp = (StackPanel) sender;
            if (!(sp.DataContext is SymbolModel sm)) return;
            var pos = e.GetPosition(SymbolViewControl);
            var mouseMovement = new Point(pos.X - _relativePoint.X, pos.Y - _relativePoint.Y);
            var canvasMovement = new Point(sm.CanvasLeft - _startCanvasPoint.X, sm.CanvasTop - _startCanvasPoint.Y);
            if (pos.X > 0)
                sm.CanvasLeft += mouseMovement.X - canvasMovement.X;
            if (pos.Y > 0)
                sm.CanvasTop += mouseMovement.Y - canvasMovement.Y;
        }

        /// <summary>
        ///     Grab onto the symbol clicked
        /// </summary>
        /// <param name="sender">The container of the symbol clicked</param>
        /// <param name="e">The mouse args</param>
        private void SymbolContainer_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _symbolHit = true;
            var sp = (StackPanel) sender;
            if (!(sp.DataContext is SymbolModel sm)) return;
            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                SelectSymbol(sm);

            if (((App) Application.Current).IsRunMode)
            {
                var rand = new Random();
                sm.Value = rand.Next(2);
            }
            else
            {
                _startCanvasPoint = new Point(sm.CanvasLeft, sm.CanvasTop);
                _relativePoint = e.GetPosition(SymbolViewControl);
                sp.CaptureMouse();
            }
        }

        /// <summary>
        ///     Set symbol to selected
        /// </summary>
        /// <param name="sm">Symbol to select</param>
        private void SelectSymbol(SymbolModel sm)
        {
            //_symbolViewModel.MainViewModel.SelectedSymbol = null;
            _symbolViewModel.MainViewModel.SelectedSymbol = sm;
        }

        /// <summary>
        ///     Release capture of symbol when left mouse button isn't pressed anymore
        /// </summary>
        /// <param name="sender">The container of the symbol</param>
        /// <param name="e">The mouse args</param>
        private void SymbolContainer_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var sp = (StackPanel) sender;
            sp.ReleaseMouseCapture();
            _symbolHit = false;
        }


        /// <summary>
        ///     Show a popup if there are unsaved chages when clicked outside of the edit area
        ///     (TODO: deside to keep/fix this or just let the editable area stay up till save or cancel is pressed)
        /// </summary>
        /// <param name="sender">The clicked object</param>
        /// <param name="e">The mouse args</param>
        private void Grid_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_symbolHit) return;
            if (_symbolViewModel.MainViewModel.SymbolTabViewModel.UnsavedChanges.Count > 0)
            {
                _symbolViewModel.MainViewModel.CreatePopUp(); //Create popup if there are unsaved changes
                return;
            }
            _symbolViewModel.MainViewModel.SelectedSymbol = null;
        }

        #endregion Methods
    }
}