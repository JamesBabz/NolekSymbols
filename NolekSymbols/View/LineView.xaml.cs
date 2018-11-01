using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using NolekSymbols.Model;
using NolekSymbols.ViewModel;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for LineView.xaml
    /// </summary>
    public partial class LineView
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public LineView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private bool _createAngleStarted;
        private LineViewModel _dataContext;
        private CustomLineModel _draggedLine;
        private Window _mainWindow;

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Creates a new line on click and hold to simulate an angle on the current line
        /// </summary>
        /// <param name="sender">The clicked line</param>
        /// <param name="e">Mouse event</param>
        private void Line_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Line line)
                || !(line.DataContext is CustomLineModel cLine)) return;
            _draggedLine = cLine;

            var mousePos = e.GetPosition(this);
            var newLine = new CustomLineModel
            {
                From = mousePos,
                To = cLine.To
            };
            cLine.To = mousePos;
            _dataContext.Lines.Add(newLine);

            //Color change
            var rnd = new Random();
            var b = new byte[3];
            rnd.NextBytes(b);
            var color = Color.FromRgb(b[0], b[1], b[2]);
            foreach (var currLine in _dataContext.Lines)
                currLine.Color = color;
            newLine.Color = color;
            //End color change

            CaptureMouse();
            _createAngleStarted = true;
        }

        /// <summary>
        ///     On view loaded
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void LineView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _dataContext = DataContext as LineViewModel;
            _mainWindow = Application.Current.MainWindow;
            if (_mainWindow != null) _mainWindow.PreviewMouseLeftButtonUp += MainWindow_OnMouseLeftButtonUp;
        }

        /// <summary>
        ///     On mosue release
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="mouseButtonEventArgs">Arguments</param>
        private void MainWindow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ReleaseMouseCapture();


            Console.WriteLine(@"Mouse up so color stuff should appear");


            _createAngleStarted = false;
        }

        /// <summary>
        ///     Sets the position of the two lines to the mouse position
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void Line_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_createAngleStarted) return;
            _draggedLine.To = e.GetPosition(this);
            _dataContext.Lines[_dataContext.Lines.Count - 1].From = e.GetPosition(this);
        }

        // NOT WORKING
        // TODO Create new line after drag has started. That way this method will fire after click.
        private void Line_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(
                @"TODO Create new line after drag has started. That way this method will fire after click.");
        }

        #endregion Methods
    }
}