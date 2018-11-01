using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml.Serialization;

namespace NolekSymbols.Model
{
    public class ColorDetermineModel : INotifyPropertyChanged
    {
        #region Properties

        public string Expression { get; set; }
        public string[] StringValues { get; set; } = new string[2];

        [XmlIgnore]
        public List<SymbolShapeModel> Shapes { get; set; }

        public string ShapesAsString { get; set; } //StringValues joined by ',' for use in CheckComboBox

        private string _operator { get; set; }

        public string Operator
        {
            get => _operator;
            set
            {
                _operator = value;
                NotifyPropertyChanged("Operator");
            }
        }

        private Color _color { get; set; }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                NotifyPropertyChanged("Color");
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public ColorDetermineModel()
        {
            Shapes = new List<SymbolShapeModel>();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="expression">
        ///     The expression to be true for the color to show. Operator first
        ///     (except for range value where it's in between the two numbers)
        /// </param>
        /// <param name="color">The color to show if the expression is true</param>
        public ColorDetermineModel(string expression, Color color)
        {
            SetProperties(expression, color);
            Shapes = new List<SymbolShapeModel>();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="expression">
        ///     The expression to be true for the color to show. Operator first
        ///     (except for range value where it's in between the two numbers)
        /// </param>
        /// <param name="color">The color to show if the expression is true</param>
        /// <param name="shape">The affected shape</param>
        public ColorDetermineModel(string expression, Color color, SymbolShapeModel shape)
        {
            SetProperties(expression, color);
            Shapes = new List<SymbolShapeModel> {shape};
            ShapesAsString = string.Join(",", Shapes);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="expression">
        ///     The expression to be true for the color to show. Operator first
        ///     (except for range value where it's in between the two numbers)
        /// </param>
        /// <param name="color">The color to show if the expression is true</param>
        /// <param name="shapes">The affected shapes</param>
        public ColorDetermineModel(string expression, Color color, List<SymbolShapeModel> shapes)
        {
            SetProperties(expression, color);
            Shapes = shapes;
            ShapesAsString = string.Join(",", Shapes);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Sets all the properties (except the shapes)
        /// </summary>
        /// <param name="expression">The expression from the constructor</param>
        /// <param name="color">The color from the constructor</param>
        private void SetProperties(string expression, Color color)
        {
            Expression = expression;
            Color = color;
            SplitExpression(expression);
        }

        /// <summary>
        ///     Splits the expression into an array and sets the Operator and StringValues
        /// </summary>
        /// <param name="expression">The expression from constructor</param>
        private void SplitExpression(string expression)
        {
            var result = Regex.Split(expression, @"(>=|=|<=|-|<|>)"); //Split on either of these seperated by pipe
            if (result[0].Equals("default") || result[0].Equals("closed"))
            {
                StringValues[0] = Operator = result[0].First().ToString().ToUpper() + result[0].Substring(1);
                return;
            }
            if (result[0].Equals("open"))
            {
                StringValues[0] = "1";
                Operator = result[0].First().ToString().ToUpper() + result[0].Substring(1);
                return;
            }
            if (result[1].Equals("-"))
            {
                StringValues[0] = result[0];
                StringValues[1] = result[2];
            }
            else
            {
                StringValues[0] = result[2];
            }
            Operator = result[1];
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