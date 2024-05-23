using Supabase;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Core.Database;

namespace DesktopApplication.MVVM.Model
{
    public class UserModel
    {
        private readonly Supabase.Client _client;

        public Action SignedIn;
        public Action SignedOut;
        public Action UserDataChanged;

        public Core.Database.User UserData { get; private set; }
        public UserModel(Supabase.Client client)
        {
            _client = client;
            SignedIn += SetUserData;
        }

        public async void SignIn()
        {
            var authState = await _client.Auth.SignIn(Constants.Provider.Discord,
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
                        <h1>Эту страницу можно закрыть</h1>
                    </body>
                    </html>");

                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.Close();
                await Task.Delay(1000);
                listener.Stop();
            }
            try
            {
                await _client.Auth.ExchangeCodeForSession(authState.PKCEVerifier, code);
                SignedIn?.Invoke();
            }
            catch { }
        }

        public async void SignOut()
        {
            await _client.Auth.SignOut();
            SignedOut?.Invoke();
        }

        private async void SetUserData()
        {
            var response = await _client.From<Core.Database.User>().Where(x => x.Id == _client.Auth.CurrentUser!.Id).Get();
            UserData = response.Model;
            UserDataChanged?.Invoke();
        }
    }
}
