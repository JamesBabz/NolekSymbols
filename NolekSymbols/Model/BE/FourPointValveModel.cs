using System.Windows;

namespace NolekSymbols.Model.BE
{
    public class FourPointValveModel : BasicSymbolModel
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public FourPointValveModel()
        {
            Name = "Default Four-point Valve Name";
            IconSource = "../Images/4way-valve.png";
            Point[] descendingTrianglePoints =
                {new Point(0, 16), new Point(32, 32), new Point(0, 48), new Point(0, 16)};
            Point[] ascendingTrianglePoints =
                {new Point(32, 32), new Point(64, 16), new Point(64, 48), new Point(32, 32)};
            Point[] topTrianglePoints = {new Point(16, 0), new Point(48, 0), new Point(32, 32), new Point(16, 0)};
            Point[] bottomTrianglePoints = {new Point(16, 64), new Point(48, 64), new Point(32, 32), new Point(16, 64)};

            AddImageShape(new SymbolShapeModel("Left triangle", descendingTrianglePoints, new Point(0, 32), 1, true,
                true));
            AddImageShape(new SymbolShapeModel("Right triangle", ascendingTrianglePoints, new Point(64, 32), 1, true,
                true));
            AddImageShape(new SymbolShapeModel("Bottom triangle", bottomTrianglePoints, new Point(32, 64), 1, true,
                true));
            AddImageShape(new SymbolShapeModel("Top triangle", topTrianglePoints, new Point(32, 0), 1, true, true));
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