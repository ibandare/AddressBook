using AddressBook.Model;
using AddressBook.Service;
using AddressBook.Service.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            
            await exportService.Export(_entryService.FindAll);
            MessageBox.Show($"Exported to {type}");
        }

        private async void LoadData()
        {
            Entries.Clear();
            _entryService.DeleteAll();
            await _csvService.ReadCsv<Entry>("data.csv", _entryService.AddAll);
            await Util.ChunkedAction.Invoke(_entryService.FindAll, async items =>
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