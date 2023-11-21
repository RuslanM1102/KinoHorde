using DesktopApplication.Core.Parser;
using DesktopApplication.MVVM.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Supabase;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DesktopApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IHost? AppHost { get; private set; }

        public App()
        {
            var handler = new GlobalErrorHandler();
            RxApp.DefaultExceptionHandler = handler;
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<SeleniumParser>();
                services.AddSingleton<Client>(provider =>
                {
                    var url = "https://ezwghqlbausbbiukjipo.supabase.co";
                    var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV6d2docWxiYXVzYmJpdWtqaXBvIiwicm9sZSI6ImFub24iLCJpYXQiOjE2OTkxMDk4NzcsImV4cCI6MjAxNDY4NTg3N30.r_FxK23wjIu493aGoEzyF34VRLODDU46-_jccOoi3Vw";

                    var options = new SupabaseOptions
                    {
                        AutoConnectRealtime = true
                    };

                    var client = new Client(url, key, options);
                    return client;
                });
            })
            .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppHost!.Dispose();
            base.OnExit(e);
        }


        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            DisposeHost();
        }

        private void DisposeHost()
        {
            AppHost!.Dispose();
        }

        private class GlobalErrorHandler : IObserver<Exception>
        {
            public Action? ExceptionCatched { get; set; }
            public void OnNext(Exception error)
            {
                ExceptionCatched?.Invoke();
            }

            public void OnError(Exception error) { }

            public void OnCompleted() { }
        }
    }
}