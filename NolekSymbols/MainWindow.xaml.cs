using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using NolekSymbols.Helpers;
using NolekSymbols.Model;
using NolekSymbols.View;
using NolekSymbols.ViewModel;
using Application = System.Windows.Application;

namespace NolekSymbols
{
    public partial class MainWindow
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            ((App) Application.Current).IsRunMode = false;
            InitializeComponent();
            _mainViewModel = (MainViewModel) DataContext;
            LoadFromFile(_defaultSaveName);
            SetupFileDialogs();
        }

        #endregion Constructors

        #region Properties

        private string _defaultSaveName { get; } = "BS9CGuDJKk";

        private MainViewModel _mainViewModel { get; }
        private SaveFileDialog _saveFileDialog;
        private OpenFileDialog _openFileDialog;

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Creates a file dialog for save and load for use with the xml
        /// </summary>
        private void SetupFileDialogs()
        {
            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.FileOk += (sender, args) => SaveToFile(_saveFileDialog.FileName);
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.FileOk += (sender, args) => LoadFromFile(_openFileDialog.FileName);
        }

        /// <summary>
        ///     Loads from symbols from xml file
        /// </summary>
        /// <param name="path">The path for the xml file to load</param>
        private void LoadFromFile(string path)
        {
            var loadedList = SerializeHelper.DeSerializeObject<ObservableCollection<SymbolModel>>(path);
            if (loadedList == null) return;
            _mainViewModel.SymbolViewModel.Symbols.Clear();
            foreach (var symbolModel in loadedList)
            {
                if (symbolModel.Image != null)
                    LoadImageForSymbol(symbolModel);

                _mainViewModel.SymbolViewModel.Symbols.Add(symbolModel);
            }
        }

        /// <summary>
        ///     Sets image for loaded symbol
        /// </summary>
        /// <param name="symbolModel">The symbol with the corresponding image </param>
        private static void LoadImageForSymbol(SymbolModel symbolModel)
        {
            var rotAngle = symbolModel.Image.RotationAngle;
            symbolModel.CreateSymbolImageModel();
            symbolModel.Image.CalculateSize();
            symbolModel.Image.RotationAngle = rotAngle;
        }

        /// <summary>
        ///     Serializes symbols and saves them to an xml on given path
        /// </summary>
        /// <param name="path">The path string where the xml should be saved</param>
        private void SaveToFile(string path)
        {
            SerializeHelper.SerializeObjectWithSubClasses(_mainViewModel.SymbolViewModel.Symbols, path);
        }

        /// <summary>
        ///     Closes the app on click
        /// </summary>
        /// <param name="sender">Clicked object</param>
        /// <param name="e">Arguments</param>
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        ///     Clears layout and deletes save file on click
        /// </summary>
        /// <param name="sender">Clicked object</param>
        /// <param name="e">Arguments</param>
        private void ClearLayout_OnClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.SymbolViewModel.Symbols.Clear();
            if (File.Exists(_defaultSaveName))
                File.Delete(_defaultSaveName);
        }

        /// <summary>
        ///     Saves to default on click
        /// </summary>
        /// <param name="sender">Clicked object</param>
        /// <param name="e">Arguments</param>
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            SaveToFile(_defaultSaveName);
        }

        /// <summary>
        ///     Opens the save file dialog on click
        /// </summary>
        /// <param name="sender">Clicked object</param>
        /// <param name="e">Arguments</param>
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            _saveFileDialog.FileName = date + " exportfile";
            _saveFileDialog.ShowDialog();
        }

        /// <summary>
        ///     OPens the load file dialog on click
        /// </summary>
        /// <param name="sender">Clicked object</param>
        /// <param name="e">Arguments</param>
        private void Import_OnClick(object sender, RoutedEventArgs e)
        {
            _openFileDialog.ShowDialog();
        }

        /// <summary>
        ///     Switches between run and edit mode on click
        /// </summary>
        /// <param name="sender">Clicked object</param>
        /// <param name="e">Arguments</param>
        private void SetMode_OnClick(object sender, RoutedEventArgs e)
        {
            ((App) Application.Current).IsRunMode = !((App) Application.Current).IsRunMode;
        }

        private void DeviceConfig_OnClick(object sender, RoutedEventArgs e)
        {
            var deviceConfigView = new DeviceConfigView();
            deviceConfigView.ShowDialog();
        }

        #endregion Methods
    }
}