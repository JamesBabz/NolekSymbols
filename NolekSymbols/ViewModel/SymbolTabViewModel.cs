using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using NolekSymbols.Model;
using Xceed.Wpf.Toolkit;

namespace NolekSymbols.ViewModel
{
    public class SymbolTabViewModel : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mainViewModel">The main view model for reference</param>
        public SymbolTabViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            SelectedSymbol = mainViewModel.SelectedSymbol;
            UnsavedChanges = new List<BindingExpression>();
        }

        #endregion Constructors

        #region Properties

        public MainViewModel MainViewModel { get; }

        public ObservableCollection<ColorItem> StandardColors { get; } = new ObservableCollection<ColorItem>
        {
            new ColorItem(Colors.Gray, "Gray"),
            new ColorItem(Colors.Green, "Green"),
            new ColorItem(Colors.Yellow, "Yellow"),
            new ColorItem(Colors.Red, "Red")
        };

        //public ObservableCollection<>

        public List<BindingExpression> UnsavedChanges { get; set; }

        public bool IgnoreTabVisibility { get; set; }

        private Visibility _tabVisibility;

        public Visibility TabVisibility
        {
            get => _tabVisibility;
            set
            {
                _tabVisibility = value;
                if (value == Visibility.Collapsed)
                    SelectedTabIndex = 0;
                NotifyPropertyChanged("TabVisibility");
            }
        }

        private int _selectedTabIndex;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;

                NotifyPropertyChanged("SelectedTabIndex");
            }
        }

        private SymbolModel _selectedSymbol;

        public SymbolModel SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                _selectedSymbol = value;
                if (!IgnoreTabVisibility)
                    TabVisibility = value == null ? Visibility.Collapsed : Visibility.Visible;
                NotifyPropertyChanged("SelectedSymbol");
            }
        }

        private bool _isClickableEnabled;

        public bool IsClickableEnabled
        {
            get => _isClickableEnabled;
            set
            {
                _isClickableEnabled = value;
                NotifyPropertyChanged("IsClickableEnabled");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Saves all changes and deselects the symbol
        /// </summary>
        public void SaveAll()
        {
            if (UnsavedChanges.Count > 0)
            {
                foreach (var change in UnsavedChanges)
                {
                    change.UpdateSource();
                    if (change.ResolvedSourcePropertyName != "DesiredThroughput") continue;
                    if (SelectedSymbol is BasicSymbolModel basSym)
                        basSym.SetThroughputModel();
                }
                UnsavedChanges.Clear();
            }
            MainViewModel.SelectedSymbol = null;
            Console.WriteLine(@"Everything has been saved");
        }

        /// <summary>
        ///     Discards all changes and deselects the symbol
        /// </summary>
        public void DiscardChanges()
        {
            UnsavedChanges.Clear();
            MainViewModel.SelectedSymbol = null;
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

        #endregion
    }
}