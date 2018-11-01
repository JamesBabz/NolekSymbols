using System.Collections.Generic;

namespace NolekSymbols.Model.BE
{
    public class ValueFieldModel : SymbolModel
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public ValueFieldModel()
        {
            Name = "Value field";
            IconSource = "../Images/Textbox.png";
            ValueSteps = new List<double> {0.1, 1, 5, 10}; // default values for increase and decrease
        }

        #endregion Constructors

        #region Properties

        public bool IsInput { get; set; } = true;
        public List<double> ValueSteps { get; set; }
        public double OutputValue { get; set; } = 0;
        public string ValueFormatString { get; set; } = "N" + 5;

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}