using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace NolekSymbols.Model
{
    public class CustomLineModel : INotifyPropertyChanged
    {
        private Color _color = Colors.Green;
        private Point _from;
        private Point _to;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                NotifyPropertyChanged("Color");
            }
        }

        public Point From
        {
            get => _from;
            set
            {
                _from = value;
                NotifyPropertyChanged("From");
            }
        }

        public Point To
        {
            get => _to;
            set
            {
                _to = value;
                NotifyPropertyChanged("To");
            }
        }


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