using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NolekMoxa.Model;
using NolekSymbols.ViewModel;
using SymbolMotor.GUICommunication;

namespace NolekSymbols.View
{
    /// <summary>
    ///     Interaction logic for DeviceConfigView.xaml
    /// </summary>
    public partial class DeviceConfigView
    {
        private readonly DeviceConfigViewModel _dataContext;

        public DeviceConfigView()
        {
            InitializeComponent();
            _dataContext = (DeviceConfigViewModel) DataContext;
            var connectedDevices = new GUITagReader().GetAllConnectedDevices("Ethernet");

            _dataContext.ConnectedDevices.Clear();
            foreach (var device in connectedDevices)
                _dataContext.ConnectedDevices.Add(device);
        }

        //TODO save.
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeviceInTreeView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dataContext.SelectedDevice = (ConnectedDevice) ((TextBlock) sender).DataContext;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}