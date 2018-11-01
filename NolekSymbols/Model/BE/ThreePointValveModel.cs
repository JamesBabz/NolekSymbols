using System.Windows;

namespace NolekSymbols.Model.BE
{
    public class ThreePointValveModel : BasicSymbolModel
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public ThreePointValveModel()
        {
            Name = "Default Three-point Valve Name";
            IconSource = "../Images/3way-valve.png";
            Point[] descendingTrianglePoints = {new Point(0, 0), new Point(32, 16), new Point(0, 32), new Point(0, 0)};
            Point[] ascendingTrianglePoints =
                {new Point(32, 16), new Point(64, 0), new Point(64, 32), new Point(32, 16)};
            Point[] bottomTrianglePoints = {new Point(16, 48), new Point(48, 48), new Point(32, 16), new Point(16, 48)};
            AddImageShape(new SymbolShapeModel("Left triangle", descendingTrianglePoints, new Point(0, 16), 1, true,
                true));
            AddImageShape(new SymbolShapeModel("Right triangle", ascendingTrianglePoints, new Point(64, 16), 1, true,
                true));
            AddImageShape(new SymbolShapeModel("Bottom triangle", bottomTrianglePoints, new Point(32, 48), 1, true,
                true));
            AmountOpenAtATime = 2;
            CreateSymbolModel();
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}