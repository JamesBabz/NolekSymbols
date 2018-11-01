using System.Windows;
using System.Windows.Media;

namespace NolekSymbols.Model.BE
{
    public class ValveModel : AdvancedSymbolModel
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public ValveModel()
        {
            Name = "Default Valve Name";
            IconSource = "../Images/2way-valve.png";
            Point[] descendingTrianglePoints =
                {new Point(0, 16), new Point(32, 32), new Point(0, 48), new Point(0, 16)};
            Point[] ascendingTrianglePoints =
                {new Point(32, 32), new Point(64, 16), new Point(64, 48), new Point(32, 32)};
            var leftT = new SymbolShapeModel("Left triangle", descendingTrianglePoints, new Point(0, 32), 1, true,
                true);
            var rightT = new SymbolShapeModel("Right triangle", ascendingTrianglePoints, new Point(64, 32), 1, true,
                true);
            AddImageShape(leftT);
            AddImageShape(rightT);

            AddImageColorCondition(new ColorDetermineModel("default", Colors.Gray, GetImageShapes()));
            AddImageColorCondition(new ColorDetermineModel("=0", Colors.Red, GetImageShapes()));
            AddImageColorCondition(new ColorDetermineModel("=10", Colors.Yellow, GetImageShapes()));
            AddImageColorCondition(new ColorDetermineModel("11-15", Colors.Orange, leftT));
            AddImageColorCondition(new ColorDetermineModel("11-15", Colors.Green, rightT));
            AddImageColorCondition(new ColorDetermineModel(">=30", Colors.Green, leftT));
            AddImageColorCondition(new ColorDetermineModel(">=40", Colors.Orange, rightT));
            CreateSymbolImageModel();
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}