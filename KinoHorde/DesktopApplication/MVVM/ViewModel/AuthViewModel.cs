using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Supabase;
using Supabase.Gotrue;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Client = Supabase.Client;

namespace DesktopApplication.MVVM.ViewModel
{
    class AuthViewModel { 
        public Action? AuthSuccessed { get; set; }
        public IReactiveCommand AuthCommand { get; set; }
        
        public AuthViewModel()
        {
            AuthCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var client =
                    await ((App)App.Current).AppHost!.Services.GetRequiredService<Client>().InitializeAsync();
                var authState = await client.Auth.SignIn(Constants.Provider.Discord,
                new SignInOptions
                {
                    FlowType = Constants.OAuthFlowType.PKCE,
                });

                string code = string.Empty;

                Process.Start(new ProcessStartInfo { FileName = authState.Uri.AbsoluteUri, UseShellExecute = true });
                using (HttpListener listener = new HttpListener())
                {
                    listener.Prefixes.Add("http://localhost:3001/");
                    listener.Start();
                    var context = await listener.GetContextAsync();
                    code = context.Request.QueryString["code"];
                    context.Response.ContentType = "text/html; charset=utf-8";

                    var buffer = Encoding.UTF8.GetBytes(@"
                    <html>
                    <head>
                        <title>Авторизация</title>
                    </head>
                    <body>
                        <h1>Авторизация успешна, эту страницу можно закрыть</h1>
                    </body>
                    </html>");

                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.Close();
                    await Task.Delay(1000);
                    listener.Stop();
                }

                await client.Auth.ExchangeCodeForSession(authState.PKCEVerifier, code);
                AuthSuccessed?.Invoke();    
            });
        }
    }
}
