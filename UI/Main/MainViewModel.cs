using AddressBook.Model;
using AddressBook.Service;
using AddressBook.Service.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AddressBook.UI.Main
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICsvService _csvService;
        private readonly IEntryService<Entry> _entryService;
        private readonly IExportServiceFactory _exportServiceFactory;

        private ObservableCollection<Entry> _entries;
        private ICommand _ImportCommand;
        private ICommand _ExportToExcelCommand;
        private ICommand _ExportToXmlCommand;
        private ICommand _ApplyFilterCommand;

        public MainViewModel(
            ICsvService csvService,
            IEntryService<Entry> entityService,
            IExportServiceFactory exportServiceFactory)
        {
            _csvService = csvService;
            _entryService = entityService;
            _exportServiceFactory = exportServiceFactory;
            Entries = new ObservableCollection<Entry>();
        }

        private string _Date;
        public string Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                OnPropertyChanged();
            }
        }
        
        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged();
            }
        }

        private string _LastName;
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
                OnPropertyChanged();
            }
        }

        private string _MiddleName;
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                _MiddleName = value;
                OnPropertyChanged();
            }
        }

        private string _City;
        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                _City = value;
                OnPropertyChanged();
            }
        }

        private string _Country;
        public string Country
        {
            get
            {
                return _Country;
            }
            set
            {
                _Country = value;
                OnPropertyChanged();
            }
        }


        public ICommand ImportCommand
        {
            get
            {
                _ImportCommand ??= new RelayCommand(param => this.LoadData());
                return _ImportCommand;
            }
        }

        public ICommand ExportToExcelCommand
        {
            get
            {
                _ExportToExcelCommand ??= new RelayCommand(param => this.ExportData("Excel"));
                return _ExportToExcelCommand;
            }
        }

        public ICommand ExportToXmlCommand
        {
            get
            {
                _ExportToXmlCommand ??= new RelayCommand(param => this.ExportData("XML"));
                return _ExportToXmlCommand;
            }
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                _ApplyFilterCommand ??= new RelayCommand(param => this.RefreshData());
                return _ApplyFilterCommand;
            }
        }

        public ObservableCollection<Entry> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }


        private async void ExportData(string type)
        {
            var exportService = _exportServiceFactory.GetInstance(type);
            
            await exportService.Export(FindByPredicate);
            MessageBox.Show($"Exported to {type}");
        }

        private async void LoadData()
        {
            _entryService.DeleteAll();
            await _csvService.ReadCsv<Entry>("data.csv", _entryService.AddAll);
            await RefreshData();

        }

        private Task<List<Entry>> FindByPredicate(int skip, int take) {
            var predicate = new Entry()
            {
                Date = Date,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                City = City,
                Country = Country
            };
            return _entryService.FindAll(predicate, skip, take);
        }

        private async Task RefreshData()
        {
            Entries.Clear();

            await Util.ChunkedAction.Invoke(FindByPredicate, async items =>
            {
                foreach (var entry in items)
                {
                    await Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        //Trace.WriteLine(entry);
                        Entries.Add(entry);
                    });
                }
            });
        }
    }
}