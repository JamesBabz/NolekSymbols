using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NolekMoxa.Model;

namespace NolekSymbols.ViewModel
{
    public class DeviceConfigViewModel : INotifyPropertyChanged
    {
        private ConnectedDevice _selectedDevice;

        #region Properties

        public ObservableCollection<ConnectedDevice> ConnectedDevices { get; set; }
        public ObservableCollection<Channel> InputChannels { get; set; }
        public ObservableCollection<Channel> OutputChannels { get; set; }

        public ConnectedDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value; 
                NotifyPropertyChanged("SelectedDevice");
                SortChannelsByInOut();
            }
        }

        #endregion Properties


        #region Constructors

        public DeviceConfigViewModel()
        {
            ConnectedDevices = new ObservableCollection<ConnectedDevice>();
            InputChannels = new ObservableCollection<Channel>();
            OutputChannels = new ObservableCollection<Channel>();
        }

        #endregion Constructors


        #region Methods

        private void SortChannelsByInOut()
        {
            InputChannels.Clear();
            OutputChannels.Clear();
            foreach (var channel in SelectedDevice.Channels.Where(c => c.IsInput == true))
            {
                InputChannels.Add(channel);
            }
            foreach (var channel in SelectedDevice.Channels.Where(c => c.IsInput == false))
            {
                OutputChannels.Add(channel);
            }
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

        #endregion INotify
    }
}
