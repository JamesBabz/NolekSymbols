using System;
using System.ComponentModel;
using System.Windows.Controls;
using NolekSymbols.Helpers;

namespace NolekSymbols
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : INotifyPropertyChanged
    {
        #region Methods

        /// <summary>
        ///     Closes all Context menues
        /// </summary>
        private void CloseAllContextMenus()
        {
            var cm = VisualTreeSearchHelper.FindVisualChildren<ContextMenu>(MainWindow);
            foreach (var menu in cm)
            {
                Console.WriteLine(cm);
                menu.IsOpen = false;
            }
        }

        #endregion Methods

        #region Properties

        private bool _isRunMode;

        public bool IsRunMode
        {
            get => _isRunMode;
            set
            {
                _isRunMode = value;
                CloseAllContextMenus();
                NotifyPropertyChanged("IsRunMode");
            }
        }

        #endregion Properties

        #region Constructors

        #endregion Constructors

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