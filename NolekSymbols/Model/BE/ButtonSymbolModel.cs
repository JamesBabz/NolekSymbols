using System.Windows;

namespace NolekSymbols.Model.BE
{
    public class ButtonSymbolModel : BasicSymbolModel
    {
        public ButtonSymbolModel()
        {
            Name = "Button";
            Point[] squarePoints =
                {new Point(0, 16), new Point(64, 16), new Point(64, 48), new Point(0, 48), new Point(0, 16)};

            AddImageShape(new SymbolShapeModel("Square", squarePoints, 1, true, true));
            NamePos = TextPosition.Center;
            AmountOpenAtATime = 1;
            CreateSymbolModel();
        }
    }
}