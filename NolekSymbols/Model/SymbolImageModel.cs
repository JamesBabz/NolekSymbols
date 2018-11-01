using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NolekSymbols.Model
{
    public class SymbolImageModel : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public SymbolImageModel()
        {
            Shapes = new List<SymbolShapeModel>();
            ColorConditions = new ObservableCollection<ColorDetermineModel>();
        }

        #endregion Constructors

        #region Properties

        private Color DefaultColor { get; } = Colors.Gray; //A default color if none is set

        private List<SymbolShapeModel> _shapes { get; set; }

        public List<SymbolShapeModel> Shapes
        {
            get => _shapes;
            set
            {
                _shapes = value;
                if (value.Count > 0)
                    CalculateSize();
                NotifyPropertyChanged("Shapes");
            }
        }

        public ObservableCollection<ColorDetermineModel> ColorConditions { get; set; }


        private int _rotationAngle { get; set; }

        public int RotationAngle
        {
            get => _rotationAngle;
            set
            {
                if (value < RotationAngle && RotationAngle <= 360)
                    ReverseImageSize();
                _rotationAngle = value;

                NotifyPropertyChanged("RotationAngle");
            }
        }

        public GeometryGroup DefaultImage
        {
            get
            {
                var geo = new GeometryGroup();
                foreach (var shape in Shapes)
                    geo.Children.Add(shape.Shape);
                return geo;
            }
        }

        private Size _imageSize { get; set; }

        public Size ImageSize
        {
            get => _imageSize;
            set
            {
                _imageSize = value;
                NotifyPropertyChanged("ImageSize");
            }
        }

        #endregion Properties


        #region Methods

        /// <summary>
        ///     Removes a color condition from the image
        /// </summary>
        /// <param name="cdm">The color condition</param>
        public void RemoveColorCondition(ColorDetermineModel cdm)
        {
            ColorConditions.Remove(cdm);
        }

        /// <summary>
        ///     Rotates the image clockwise
        /// </summary>
        /// <param name="container">The container of the image to rotate</param>
        /// <param name="degrees">Degrees to rotate clockwise</param>
        public void RotateImage(UIElement container, int degrees)
        {
            RotationAngle += degrees;
            if (RotationAngle >= 360)
                RotationAngle -= 360;

            Rotate(container);
        }

        /// <summary>
        ///     The method to do the rotation
        /// </summary>
        /// <param name="container">Container to rotate</param>
        private void Rotate(UIElement container)
        {
            if (container == null) return;
            container.RenderTransform = new RotateTransform(RotationAngle,
                ImageSize.Width / 2,
                ImageSize.Height / 2);
            container.RenderTransform.Value.Rotate(RotationAngle);
            CalculateSize();
        }

        /// <summary>
        ///     Rotates image to a specific angle
        /// </summary>
        /// <param name="container">The container of the image to rotate</param>
        /// <param name="angle">The angle to rotate to</param>
        public void RotateImageTo(UIElement container, int angle)
        {
            RotationAngle = angle;
            Rotate(container);
        }

        /// <summary>
        ///     Sets the size of the image
        /// </summary>
        public void CalculateSize()
        {
            ImageSize = DefaultImage.Bounds.Size;
        }

        /// <summary>
        ///     Swaps height and width
        /// </summary>
        private void ReverseImageSize()
        {
            if (ImageSize.IsEmpty) return;
            ImageSize = new Size(ImageSize.Height, ImageSize.Width);
        }

        /// <summary>
        ///     Updates color of the shape (TODO: deside if this should be moved? Maybe inside ColorDetermineModel?)
        /// </summary>
        /// <param name="value">The read value to compare against the expressions</param>
        public void UpdateShapeColors(double value)
        {
            var updatedShapes = new List<SymbolShapeModel>(); // A list of shapes that has been updated
            foreach (var condition in ColorConditions)
                if (IsConditionMet(condition, value))
                    foreach (var shape in condition.Shapes)
                    {
                        if (shape.IsDisabled) continue;
                        shape.FillColor = condition.Color;
                        updatedShapes.Add(shape);
                    }
                else
                    // Set default. Skip the shapes that has been updated
                    foreach (var shape in condition.Shapes.Except(updatedShapes))

                        shape.FillColor = GetDefaultColor(shape);
        }

        /// <summary>
        ///     Checks if the condition matches the value
        /// </summary>
        /// <param name="condition">The ColorDetermineModel containing the expression</param>
        /// <param name="value">The current value to match</param>
        /// <returns>True if the condition matches the value</returns>
        private static bool IsConditionMet(ColorDetermineModel condition, double value)
        {
            if (condition.Expression.Equals(""))
                return false;
            var conditionMet = false;
            double.TryParse(condition.StringValues[0], out var val1);
            switch (condition.Operator)
            {
                case "=":
                    if (Math.Abs(value - val1) < 0.00001)
                        conditionMet = true;
                    break;
                case "-":
                    double.TryParse(condition.StringValues[1], out var val2);
                    if (value > val1 && value < val2)
                        conditionMet = true;
                    break;
                case "<":
                    if (value < val1)
                        conditionMet = true;
                    break;
                case ">":
                    if (value > val1)
                        conditionMet = true;
                    break;
                case "<=":
                    if (value <= val1)
                        conditionMet = true;
                    break;
                case ">=":
                    if (value >= val1)
                        conditionMet = true;
                    break;
                case "Open":
                    if ((int) value == 1)
                        conditionMet = true;
                    break;
                case "Closed":
                    if ((int) value == 0)
                        conditionMet = true;
                    break;
                case "Default":
                    return false;
                default:
                    Console.WriteLine(@"Given operator does not exist {0}", condition.Operator);
                    break;
            }
            return conditionMet;
        }

        /// <summary>
        ///     Look for a default value or use the one given at the top of this class
        /// </summary>
        /// <param name="shape">The shape with the default color</param>
        /// <returns>Either the set default value or the one found on the shape</returns>
        public Color GetDefaultColor(SymbolShapeModel shape)
        {
            var first = ColorConditions
                .FirstOrDefault(x =>
                    x.Expression.Equals("default") && x.Shapes.Contains(shape));
            return first?.Color ?? DefaultColor;
        }

        /// <summary>
        ///     Add shape to list of shapes
        /// </summary>
        /// <param name="shape">The shape to add</param>
        public void AddShape(SymbolShapeModel shape)
        {
            Shapes.Add(shape);
        }

        /// <summary>
        ///     Add multiple shapes to the list of shapes
        /// </summary>
        /// <param name="shapes">The list of shapes to add to the current shape list</param>
        public void AddShapes(List<SymbolShapeModel> shapes)
        {
            foreach (var shape in shapes)
                AddShape(shape);
        }

        /// <summary>
        ///     Add ColorCondition to list of ColorConditions
        /// </summary>
        /// <param name="colorCondition">the ColorCondition to add</param>
        public void AddColorCondition(ColorDetermineModel colorCondition)
        {
            ColorConditions.Add(colorCondition);
        }

        /// <summary>
        ///     Add multiple ColorConditions to the list of ColorConditions
        /// </summary>
        /// <param name="colorConditions">The list of ColorConditions to add to the current list of ColorConditions</param>
        public void AddColorConditions(List<ColorDetermineModel> colorConditions)
        {
            foreach (var colorCondition in colorConditions)
            {
                AddColorCondition(colorCondition);
                foreach (var shape in colorCondition.Shapes)
                    SetDefaultColor(shape);
            }
        }

        /// <summary>
        ///     Sets the default color
        /// </summary>
        /// <param name="shape">The shapes for the default color to be sat</param>
        public void SetDefaultColor(SymbolShapeModel shape)
        {
            var col = GetDefaultColor(shape);
            shape.FillColor = col;
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