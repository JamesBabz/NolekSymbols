using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Xml.Serialization;

namespace NolekSymbols.Model
{
    public abstract class SymbolModel : INotifyPropertyChanged
    {
        public enum TextPosition
        {
            Top,
            Left,
            Bottom,
            Right,
            Center
        }

        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        protected SymbolModel()
        {
            Id = DateTime.Now.Ticks;
            _imageShapes = new List<SymbolShapeModel>();
            _imageColorConditions = new List<ColorDetermineModel>();
            SetDefaultValues();
        }

        #endregion Constructors

        #region Properties

        public long Id { get; }
        public SymbolImageModel Image { get; set; }
        private List<SymbolShapeModel> _imageShapes { get; }
        private List<ColorDetermineModel> _imageColorConditions { get; }
        public TextPosition NamePos { get; set; } = TextPosition.Bottom;

        //TODO: Couple lines and color to symbol
        //public List<CustomLineModel> Line { get; set; }

        public string IconSource { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _tooltip;

        public string Tooltip
        {
            get => _tooltip;
            set
            {
                _tooltip = value;
                NotifyPropertyChanged("Tooltip");
            }
        }

        private double _value { get; set; }

        [XmlIgnore]
        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
                Image?.UpdateShapeColors(value);
            }
        }

        public double _canvasTop { get; set; }

        public double CanvasTop
        {
            get => _canvasTop;
            set
            {
                _canvasTop = value;
                NotifyPropertyChanged("CanvasTop");
            }
        }

        public double _canvasLeft { get; set; }

        public double CanvasLeft
        {
            get => _canvasLeft;
            set
            {
                _canvasLeft = value;
                NotifyPropertyChanged("CanvasLeft");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Sets the default values for the symbol
        /// </summary>
        private void SetDefaultValues()
        {
            var type = GetType().BaseType;
            if (type == null) return;
            if (type.Name.Equals("BasicSymbolModel") || type.Name.Equals("AdvancedSymbolModel"))
                CreateDefaultImage();

            IconSource = "../Images/Output.png";
            Tooltip = "This symbol has not been given a tooltip text. Click it and change it in the \"General\" tab";
        }

        /// <summary>
        ///     Creates the default image if none is set
        /// </summary>
        private void CreateDefaultImage()
        {
            Image = new SymbolImageModel();
            Point[] squarePoints =
                {new Point(0, 0), new Point(0, 64), new Point(64, 64), new Point(64, 0)};
            Point[] firstLine = {new Point(0, 0), new Point(64, 64)};
            Point[] secondLine = {new Point(64, 0), new Point(0, 64)};
            Image.Shapes.Add(new SymbolShapeModel("Square", squarePoints, 1, true, true));
            Image.Shapes.Add(new SymbolShapeModel("LeftToRightLine", firstLine, 1, false, false));
            Image.Shapes.Add(new SymbolShapeModel("RightToLeftLine", secondLine, 1, false, false));
        }

        /// <summary>
        ///     Creates the image
        /// </summary>
        public void CreateSymbolImageModel()
        {
            if (_imageShapes.Count < 1) return;
            Image = new SymbolImageModel();
            foreach (var shape in _imageShapes)
                Image.Shapes.Add(shape);
            Image.AddColorConditions(_imageColorConditions);
        }

        /// <summary>
        ///     Add shape to be added to the image
        /// </summary>
        /// <param name="shape">The shape model</param>
        public void AddImageShape(SymbolShapeModel shape)
        {
            _imageShapes.Add(shape);
        }

        /// <summary>
        ///     Gets all the image shapes
        /// </summary>
        /// <returns>A list of image shapes</returns>
        public List<SymbolShapeModel> GetImageShapes()
        {
            return _imageShapes;
        }

        /// <summary>
        ///     Add color condition for image
        /// </summary>
        /// <param name="colCon">The color condition</param>
        public void AddImageColorCondition(ColorDetermineModel colCon)
        {
            _imageColorConditions.Add(colCon);
        }

        /// <summary>
        ///     Gets the image color conditions
        /// </summary>
        /// <returns>A list of color conditions</returns>
        public List<ColorDetermineModel> GetImageColorConditions()
        {
            return _imageColorConditions;
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