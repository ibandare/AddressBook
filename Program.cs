using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AddressBook.UI.Main;
using AddressBook.Model;
using AddressBook.Service.Impl;
using AddressBook.Service;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace AddressBook
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {

            var host = Host.CreateDefaultBuilder()

                .ConfigureServices(services =>
                {
                    services.AddDbContextPool<ApplicationContext>((options) =>
                    {
                        options.UseSqlServer(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
                    });
                    services.AddSingleton<ICsvService, CsvService>();
                    services.AddSingleton<IEntryService<Entry>, EntryService>();
                    services.AddSingleton<IExportService, ExcelExportService>();
                    services.AddSingleton<IExportService, XmlExportService>();
                    services.AddSingleton<IExportServiceFactory, ExportServiceFactory>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainWindow>();
                    services.AddTransient<App>();
                })
                .Build();

            var app = host.Services.GetService<App>();

            app?.Run();
        }
    }
}
