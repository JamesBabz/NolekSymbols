using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using NolekSymbols.Helpers;
using NolekSymbols.Model;
using NolekSymbols.Model.BE;
using NolekSymbols.ViewModel;
using SymbolMotor;
using Xceed.Wpf.Toolkit.Core.Converters;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for SymbolControl.xaml
    /// </summary>
    public partial class SymbolControl : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public SymbolControl()
        {
            InitializeComponent();
            Loaded += SymbolControl_Loaded;
            _circles = new List<Ellipse>();
        }

        #endregion Constructors

        #region Properties

        private CustomLineModel _currentLine;
        private SymbolModel _dataContext;
        private bool _drawStarted;
        private Window _mainWindow;
        private MainViewModel _mvm;
        private SymbolView _symbolView;
        private readonly List<Ellipse> _circles;

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Draw the symbol when it has been fully loaded
        /// </summary>
        private void SymbolControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;

            _dataContext = DataContext as SymbolModel;
            var vfm = _dataContext as ValueFieldModel;
            if (_dataContext != null)
            {
                var tb = CreateNameBlock(_dataContext);
                switch (_dataContext.NamePos)
                {
                    case SymbolModel.TextPosition.Top:
                        MyStackPanel.Children.Add(tb);
                        if (_dataContext.Image != null) CreateImageForSymbol(_dataContext);
                        if (vfm != null) CreateValueField(vfm);
                        break;
                    case SymbolModel.TextPosition.Left:
                        throw new NotImplementedException();
                        break;
                    case SymbolModel.TextPosition.Bottom:
                        if (_dataContext.Image != null) CreateImageForSymbol(_dataContext);
                        if (vfm != null) CreateValueField(vfm);
                        MyStackPanel.Children.Add(tb);
                        break;
                    case SymbolModel.TextPosition.Right:
                        throw new NotImplementedException();
                        break;
                    case SymbolModel.TextPosition.Center:
                        if (_dataContext.Image != null) CreateImageForSymbol(_dataContext);
                        if (vfm != null) CreateValueField(vfm);
                        var g = MyStackPanel.Children[0] as Grid;
                        tb.VerticalAlignment = VerticalAlignment.Center;
                        g.Children.Add(tb);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _mainWindow = Application.Current.MainWindow;
            if (_mainWindow == null) return;
            _mvm = _mainWindow.DataContext as MainViewModel;

            _mainWindow.MouseMove += MainWindow_OnMouseMove;
            _mainWindow.MouseUp += MainWindow_OnMouseUp;

            _symbolView = VisualTreeSearchHelper.FindParent<SymbolView>(this);
        }

        /// <summary>
        ///     Hides all circles when left mouse button is up
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="mouseButtonEventArgs">Arguments</param>
        private void MainWindow_OnMouseUp(object o, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!_drawStarted) return;
            _drawStarted = false;
            _mvm.SymbolTabViewModel.IgnoreTabVisibility = false;
            foreach (var circle in _circles)
                circle.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Sets the angle of the line being dragged
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="mouseEventArgs">Mouae arguments</param>
        private void MainWindow_OnMouseMove(object o, MouseEventArgs mouseEventArgs)
        {
            if (!_drawStarted) return;
            _currentLine.To = mouseEventArgs.GetPosition(_symbolView);
        }


        /// <summary>
        ///     Creates the value field for ValueFieldModels
        /// </summary>
        /// <param name="vfm">The valuefieldmodel</param>
        private void CreateValueField(ValueFieldModel vfm)
        {
            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1)
            };
            var txtBlock = new TextBlock();
            RegisterName("valueTb", txtBlock);

            var valueBinding = vfm.IsInput
                ? new Binding("Value")
                {
                    Source = vfm,
                    NotifyOnTargetUpdated = true
                }
                : new Binding("OutputValue")
                {
                    Source = vfm,
                    NotifyOnTargetUpdated = true
                };
            border.Child = txtBlock;
            txtBlock.SetBinding(TextBlock.TextProperty, valueBinding); // Bind value to TextBlock
            MyStackPanel.Children.Add(border);
        }

        /// <summary>
        ///     Creates the textblock containing the symbol name
        /// </summary>
        /// <param name="sm">The symbol</param>
        private TextBlock CreateNameBlock(SymbolModel sm)
        {
            var nameTextBlock = new TextBlock { HorizontalAlignment = HorizontalAlignment.Center };
            RegisterName("tbName", nameTextBlock);
            var nameBinding = new Binding("Name")
            {
                Source = sm,
                NotifyOnTargetUpdated = true
            };
            nameTextBlock.SetBinding(TextBlock.TextProperty, nameBinding); // Bind name to TextBlock

            // Add SizeChanged method for when text has changed
            nameTextBlock.SizeChanged += NameTextBlockOnSizeChanged;

            return nameTextBlock; // Add to StackPanel
        }

        /// <summary>
        ///     Creates the image for the symbol
        /// </summary>
        /// <param name="sm">The symbol</param>
        private void CreateImageForSymbol(SymbolModel sm)
        {
            var gridForImage = new Grid
            {
                Name = "MyGrid",
                Width = double.NaN,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            foreach (var shape in sm.Image.Shapes)
            {
                var p = new Path
                {
                    Data = shape.Shape, // New path for every shape for different colors
                    Stroke = Brushes.Black, // Draw it
                    Tag = shape.LinePoint // Outline it
                };

                var colorBind = new Binding("FillColor")
                {
                    Converter = new ColorToSolidColorBrushConverter(),
                    Source = shape
                };
                p.SetBinding(Shape.FillProperty, colorBind);

                gridForImage.Children.Add(p); // Add to Grid
                if (!(shape.LinePoint.X >= 0) || !(shape.LinePoint.Y >= 0)) continue;
                p.MouseEnter += Shape_OnMouseEnter;
                p.MouseLeave += Shape_OnMouseLeave;
                AddCircle(p);
            }

            gridForImage.Height = 64;
            if (sm.Image.RotationAngle != 0)
                sm.Image.RotateImageTo(gridForImage, sm.Image.RotationAngle);
            MyStackPanel.Children.Add(gridForImage); // Add to StackPanel
        }

        /// <summary>
        ///     Creates the circles to be shown on hover
        /// </summary>
        /// <param name="p">The path of the shape</param>
        private void AddCircle(Path p)
        {
            const int circleOffset = 1;
            var circle = new Ellipse
            {
                Width = 12,
                Height = 12,
                Fill = new SolidColorBrush(Colors.Black)
            };
            var g = p.Parent as Grid;
            var linePoint = (Point)p.Tag;
            circle.RenderTransform = new TranslateTransform(linePoint.X - p.Data.Bounds.Width,
                linePoint.Y - p.Data.Bounds.Height - circleOffset);
            circle.PreviewMouseLeftButtonDown += Circle_OnMouseDown;
            circle.PreviewMouseLeftButtonUp += (sender, args) => _drawStarted = false;
            circle.MouseEnter += (sender, args) =>
            {
                if (sender is Ellipse c) c.Visibility = Visibility.Visible;
            };

            circle.MouseLeave += Circle_OnMouseLeave;
            g?.Children.Add(circle);
            circle.Visibility = Visibility.Hidden;
            _circles.Add(circle);
        }

        /// <summary>
        ///     Starts drawing from the circle
        /// </summary>
        /// <param name="sender">The circle</param>
        /// <param name="mouseButtonEventArgs">Arguments</param>
        private void Circle_OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!(sender is Ellipse c)
                || !(c.Parent is Grid g)
                || Keyboard.IsKeyDown(Key.LeftCtrl)) return;
            var i = g.Children.IndexOf(c);
            _mvm.SymbolTabViewModel.IgnoreTabVisibility = true;
            var sd = (((StackPanel)g.Parent).ActualWidth - _dataContext.Image.DefaultImage.Bounds.Width) / 2;
            if (g.Children[i - 1] is Path p)
            {
                var tagPoint = (Point)p.Tag;
                var fromPoint = new Point
                {
                    X = _dataContext.CanvasLeft + tagPoint.X + sd,
                    Y = _dataContext.CanvasTop + tagPoint.Y
                };
                _currentLine = new CustomLineModel { From = fromPoint };
            }
            _mvm.LineViewModel.Lines.Add(_currentLine);

            _drawStarted = true;
        }

        /// <summary>
        ///     Hides the circle when the mouse leaves the shape
        /// </summary>
        /// <param name="sender">The path of the shape</param>
        /// <param name="mouseEventArgs">Arguments</param>
        private static void Shape_OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!(sender is Path p)
                || p.Parent == null
                || !(p.Parent is Grid g)) return;
            var i = g.Children.IndexOf(p);
            g.Children[i + 1].Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Shows the circle when the mouse enters the shape
        /// </summary>
        /// <param name="sender">The path of the shape</param>
        /// <param name="mouseEventArgs">Arguments</param>
        private static void Shape_OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!(sender is Path p)
                || !(p.Parent is Grid g)
                || ((App)Application.Current).IsRunMode) return;
            var i = g.Children.IndexOf(p);
            g.Children[i + 1].Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Hiddes circle on leave
        /// </summary>
        /// <param name="sender">The circle</param>
        /// <param name="mouseEventArgs">Mouse event</param>
        private static void Circle_OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed) return;
            if (sender is Ellipse circle) circle.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Only allows context menu to be opened if edit mode is active
        /// </summary>
        /// <param name="sender">The Contextmenu</param>
        /// <param name="e">Arguments</param>
        private void ContextMenu_OnOpened(object sender, RoutedEventArgs e)
        {
            if (!((App)Application.Current).IsRunMode
                || !(sender is ContextMenu cm)) return;
            cm.IsOpen = false;
        }

        /// <summary>
        ///     Resizes the textblock when the name changes
        /// </summary>
        /// <param name="sender">The Textblock</param>
        /// <param name="sizeChangedEventArgs">Argmuents containing new and old sizes</param>
        private void NameTextBlockOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (sizeChangedEventArgs.PreviousSize == new Size(0, 0)
                || !(DataContext is SymbolModel symbol)
                || symbol.Image == null)
                return;

            var oldWidth = sizeChangedEventArgs.PreviousSize.Width;
            var newWidth = sizeChangedEventArgs.NewSize.Width;
            var imageWidth = symbol.Image.ImageSize.Width;

            if (oldWidth < imageWidth && newWidth < imageWidth) return;

            if (oldWidth > imageWidth && newWidth < imageWidth)
            {
                symbol.CanvasLeft -= (imageWidth - oldWidth) / 2;
                return;
            }
            if (oldWidth < imageWidth && newWidth > imageWidth)
            {
                symbol.CanvasLeft += (imageWidth - newWidth) / 2;
                return;
            }
            symbol.CanvasLeft += (oldWidth - newWidth) / 2;
        }

        /// <summary>
        ///     Rotates the image on click
        /// </summary>
        /// <param name="sender">The menu item</param>
        /// <param name="e">Arguments</param>
        private void Rotate_OnClick(object sender, RoutedEventArgs e)
        {
            GetSymbolFromSender(sender).Image.RotateImage(GetGridFromSender(sender),
                int.Parse((string)ConvertSenderToMenuItem(sender).Tag));
        }

        /// <summary>
        ///     Sets a random value. Mainly used for testing
        /// </summary>
        /// <param name="sender">The menu item</param>
        /// <param name="e">Arguments</param>
        private void RandomValue_OnClick(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            var randInt = rand.Next(50);
            Console.WriteLine(randInt);
            GetSymbolFromSender(sender).Value = randInt;
        }

        /// <summary>
        ///     Converts the sender to a menuitem
        /// </summary>
        /// <param name="sender">The menuitem</param>
        /// <returns>The menuitem</returns>
        private static MenuItem ConvertSenderToMenuItem(object sender)
        {
            return sender as MenuItem;
        }

        /// <summary>
        ///     Gets the context menu containing the menuitem
        /// </summary>
        /// <param name="sender">The menuitem</param>
        /// <returns>The contextmenu</returns>
        private static ContextMenu GetContextMenuFromSender(object sender)
        {
            return ConvertSenderToMenuItem(sender).Parent as ContextMenu;
        }

        /// <summary>
        ///     Gets the stackpanel from the symbol
        /// </summary>
        /// <param name="sender">The symbol</param>
        /// <returns>The stackpanel</returns>
        private static StackPanel GetStackPanelFromSender(object sender)
        {
            return GetContextMenuFromSender(sender).PlacementTarget as StackPanel;
        }

        /// <summary>
        ///     Get grid from the stackpanel
        /// </summary>
        /// <param name="sender">The stackpanel</param>
        /// <returns>The grid</returns>
        private static Grid GetGridFromSender(object sender)
        {
            return GetStackPanelFromSender(sender).Children[0] as Grid;
        }

        /// <summary>
        ///     Gets the symbol from the stackpanel
        /// </summary>
        /// <param name="sender">The stackpanel</param>
        /// <returns>The symbolmodel</returns>
        private static SymbolModel GetSymbolFromSender(object sender)
        {
            return GetStackPanelFromSender(sender).DataContext as SymbolModel;
        }

        /// <summary>
        ///     Sets the value on click
        /// </summary>
        /// <param name="sender">The menuitem</param>
        /// <param name="e">Arguments</param>
        private void SetValue_OnClick(object sender, RoutedEventArgs e)
        {
            var value = int.Parse((string)ConvertSenderToMenuItem(sender).Tag);
            GetSymbolFromSender(sender).Value = value;
        }

        #endregion Methods

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