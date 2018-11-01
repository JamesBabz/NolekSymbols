using System.Collections.ObjectModel;
using NolekSymbols.Model;

namespace NolekSymbols.ViewModel
{
    public class LineViewModel
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mainViewModel"></param>
        public LineViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Lines = new ObservableCollection<CustomLineModel>();
        }

        #endregion Constructors

        #region Properties

        public MainViewModel MainViewModel { get; }

        public ObservableCollection<CustomLineModel> Lines { get; set; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}