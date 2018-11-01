using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using NolekMoxa.Model;

namespace NolekSymbols.ViewModel
{
    public class DeviceConfigViewModel : INotifyPropertyChanged
    {

        #region Properties

        private ConnectedDevice _selectedDevice;

        public ObservableCollection<ConnectedDevice> ConnectedDevices { get; set; }
        public ObservableCollection<Channel> InputChannels { get; set; }
        public ObservableCollection<Channel> OutputChannels { get; set; }

        public ConnectedDevice SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                NotifyPropertyChanged("SelectedDevice");
                SortChannelsByInOut(); // Sort channels for the device
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

        /// <summary>
        ///     Sorts the channels between in- and outputs
        /// </summary>
        private void SortChannelsByInOut()
        {
            InputChannels.Clear();
            OutputChannels.Clear();
            foreach (var channel in SelectedDevice.Channels.Where(c => c.IsInput == true))
                InputChannels.Add(channel);
            foreach (var channel in SelectedDevice.Channels.Where(c => c.IsInput == false))
                OutputChannels.Add(channel);
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