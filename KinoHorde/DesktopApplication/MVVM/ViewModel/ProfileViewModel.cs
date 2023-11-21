using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Supabase;
using Postgrest;
using System;
using DesktopApplication.Core.Database;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI.Fody.Helpers;

namespace DesktopApplication.MVVM.ViewModel
{
    class ProfileViewModel : ReactiveObject
    {
        private Supabase.Client _client = ((App)App.Current).AppHost!.Services.GetRequiredService<Supabase.Client>();
        public Action LogOuted { get; set; }
        public IReactiveCommand LogOutCommand { get; set; }
        [Reactive] 
        public User User { get; set; }
        public ProfileViewModel()
        {
            LogOutCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                _client.Auth.SignOut();
                LogOuted?.Invoke();
            });
        }

        public async void SetUser()
        {
            var response = await _client.From<User>().Where(x => x.IdSp == _client.Auth.CurrentUser!.Id).Get();
            User = response.Model;
        }
    }
}
