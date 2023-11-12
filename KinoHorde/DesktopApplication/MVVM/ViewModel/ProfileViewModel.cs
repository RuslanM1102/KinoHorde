using DesktopApplication.Core.Database;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DesktopApplication.MVVM.ViewModel
{
    class ProfileViewModel : ReactiveObject
    {
        public Action AuthSuccessed { get; set; }
        public IReactiveCommand AuthCommand { get; set; }
        public ProfileViewModel()
        {
            AuthCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                //var authState = await SupabaseContext.Client.Auth.SignIn(Constants.Provider.Discord,
                //new SignInOptions
                //{
                //    FlowType = Constants.OAuthFlowType.PKCE,
                //});

                //string code = string.Empty;

                //Process.Start(new ProcessStartInfo { FileName = authState.Uri.AbsoluteUri, UseShellExecute = true });
                //using (HttpListener listener = new HttpListener())
                //{
                //    listener.Prefixes.Add("http://localhost:3001/");
                //    listener.Start();
                //    var context = await listener.GetContextAsync();
                //    code = context.Request.QueryString["code"];
                //    listener.Stop();
                //}

                //await SupabaseContext.Client.Auth.ExchangeCodeForSession(authState.PKCEVerifier, code);
                AuthSuccessed?.Invoke();
            });
        }
    }
}
