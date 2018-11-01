using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NolekSymbols.Model
{
    public class SymbolShapeModel : INotifyPropertyChanged
    {
        #region Properties

        public int StrokeThickness { get; set; }
        public string ShapeName { get; set; }
        public StreamGeometry Shape { get; set; }
        public bool IsFilled { get; set; }
        public bool IsClosed { get; set; }
        public bool IsDisabled { get; set; }
        public Point LinePoint { get; set; }

        private Color _fillColor { get; set; }

        //[XmlIgnore]
        public Color FillColor
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
                NotifyPropertyChanged("FillColor");
            }
        }

        private Point[] _shapePoints { get; set; }

        public Point[] ShapePoints
        {
            get => _shapePoints;
            set
            {
                _shapePoints = value;
                CreateShape();
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public SymbolShapeModel()
        {
            FillColor = new Color();
            //CreateShape();
        }

        /// <summary>
        ///     Create a shape from points
        /// </summary>
        /// <param name="shapeName">The name of the shape for future reference</param>
        /// <param name="points">The points used to draw the shape</param>
        /// <param name="linePoint">Where the line should connect</param>
        /// <param name="strokeThickness">The thickness of the outer line</param>
        /// <param name="isFilled">Should it be filled with a color</param>
        /// <param name="isClosed">Is it a closed or open shape</param>
        /// <returns>The freshly created shape</returns>
        public SymbolShapeModel(string shapeName, Point[] points, Point linePoint, int strokeThickness, bool isFilled,
            bool isClosed)
        {
            ShapeName = shapeName;
            StrokeThickness = strokeThickness;
            IsFilled = isFilled;
            IsClosed = isClosed;
            ShapePoints = points;
            LinePoint = linePoint;
            //CreateShape();
        }

        /// <summary>
        ///     Create a shape from points
        /// </summary>
        /// <param name="shapeName">The name of the shape for future reference</param>
        /// <param name="points">The points used to draw the shape</param>
        /// <param name="strokeThickness">The thickness of the outer line</param>
        /// <param name="isFilled">Should it be filled with a color</param>
        /// <param name="isClosed">Is it a closed or open shape</param>
        /// <returns>The freshly created shape</returns>
        public SymbolShapeModel(string shapeName, Point[] points, int strokeThickness, bool isFilled,
            bool isClosed)
        {
            ShapeName = shapeName;
            StrokeThickness = strokeThickness;
            IsFilled = isFilled;
            IsClosed = isClosed;
            ShapePoints = points;
            LinePoint = new Point(-1, -1);
            //CreateShape();
        }

        #endregion Constructors

        #region Methods

        public override string ToString()
        {
            return ShapeName;
        }

        /// <summary>
        ///     Draws the shape
        /// </summary>
        private void CreateShape()
        {
            var geometry = new StreamGeometry {FillRule = FillRule.EvenOdd};

            using (var ctx = geometry.Open())
            {
                ctx.BeginFigure(ShapePoints[0], IsFilled, IsClosed);
                foreach (var point in ShapePoints.Skip(1))
                    // Draw a line to the next specified point.
                    ctx.LineTo(point, true /* is stroked */, false /* is smooth join */);
            }
            Shape = geometry;
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