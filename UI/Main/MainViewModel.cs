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
        private ICommand _importCommand;
        private ICommand _exportToExcelCommand;
        private ICommand _exportToXmlCommand;
        private ICommand _applyFilterCommand;

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

        private string _date;
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }
        
        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _middleName;
        public string MiddleName
        {
            get
            {
                return _middleName;
            }
            set
            {
                _middleName = value;
                OnPropertyChanged();
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }


        public ICommand ImportCommand
        {
            get
            {
                _importCommand ??= new RelayCommand(param => this.LoadData());
                return _importCommand;
            }
        }

        public ICommand ExportToExcelCommand
        {
            get
            {
                _exportToExcelCommand ??= new RelayCommand(param => this.ExportData("Excel"));
                return _exportToExcelCommand;
            }
        }

        public ICommand ExportToXmlCommand
        {
            get
            {
                _exportToXmlCommand ??= new RelayCommand(param => this.ExportData("XML"));
                return _exportToXmlCommand;
            }
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                _applyFilterCommand ??= new RelayCommand(param => this.RefreshData());
                return _applyFilterCommand;
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