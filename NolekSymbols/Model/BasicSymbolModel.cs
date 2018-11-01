using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using NolekSymbols.Helpers;

namespace NolekSymbols.Model
{
    public abstract class BasicSymbolModel : SymbolModel
    {
        #region Properties

        public int AmountOpenAtATime { get; set; }

        private List<SymbolImageModel> _availableCombinationsOfShape { get; set; }

        public int DesiredThroughput { get; set; } = 0;

        private bool _isOpen;

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                Value = value ? 1 : 0;
                NotifyPropertyChanged("IsOpen");
            }
        }

        [XmlIgnore]
        public List<SymbolImageModel> AvailableCombinationsOfShape
        {
            get => _availableCombinationsOfShape;
            set
            {
                _availableCombinationsOfShape = value;
                NotifyPropertyChanged("AvailableCombinationsOfShape");
            }
        }

        #endregion Properties

        #region Constructors

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Gets SymbolImage dependent on the desired throughput
        /// </summary>
        /// <returns>The symbolimage</returns>
        public SymbolImageModel GetDesiredThroughput()
        {
            return _availableCombinationsOfShape[DesiredThroughput];
        }

        /// <summary>
        ///     Disable shapes and updates colors based on the selected throughput
        /// </summary>
        public void SetThroughputModel()
        {
            for (var i = 0; i < GetDesiredThroughput().Shapes.Count; i++)
                Image.Shapes[i].IsDisabled = GetDesiredThroughput().Shapes[i].IsDisabled;
            Image.UpdateShapeColors(Value);
        }

        /// <summary>
        ///     Creates default basic symbol
        /// </summary>
        public void CreateSymbolModel()
        {
            AddImageColorCondition(new ColorDetermineModel("open", Colors.Green, GetImageShapes()));
            AddImageColorCondition(new ColorDetermineModel("closed", Colors.Red, GetImageShapes()));
            CreateSymbolImageModel();
            AvailableCombinationsOfShape = new List<SymbolImageModel>();

            var combinations = MathHelper.GetCombinationsWithoutRepetition(AmountOpenAtATime, Image.Shapes.Count);

            foreach (var combination in combinations)
            {
                var r = CombineShapes(GetImageShapes(), combination);
                AvailableCombinationsOfShape.Add(r);
            }
            SetThroughputModel();
        }

        /// <summary>
        ///     Combines all the shapes for use in the throughput combobox
        /// </summary>
        /// <param name="shapes">The shapes to combine</param>
        /// <param name="numbersNotToFill">An int array with the numbers to not fill</param>
        /// <returns>The symbol image for the combobox</returns>
        private static SymbolImageModel CombineShapes(IEnumerable<SymbolShapeModel> shapes, int[] numbersNotToFill)
        {
            var image = new SymbolImageModel();
            var shapeList = new List<SymbolShapeModel>();
            var i = 1;
            foreach (var shape in shapes)
            {
                var newShape = new SymbolShapeModel("Temp" + shape.ShapeName, shape.ShapePoints, shape.LinePoint, 1,
                    shape.IsFilled,
                    shape.IsClosed);
                if (!numbersNotToFill.Contains(i))
                {
                    newShape.FillColor = Colors.Black;
                    newShape.IsDisabled = true;
                }

                shapeList.Add(newShape);
                i++;
            }
            image.AddShapes(shapeList);
            image.CalculateSize();
            return image;
            //return shapeList;
        }

        #endregion Methods
    }
}