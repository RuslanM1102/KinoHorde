using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Supabase;
using Postgrest;
using System;
using DesktopApplication.Core.Database;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using DesktopApplication.MVVM.Model;

namespace DesktopApplication.MVVM.ViewModel
{
    class ProfileViewModel : ReactiveObject
    {
        public IReactiveCommand LogOutCommand { get; set; }
        [Reactive] 
        public User User { get; private set; }

        private readonly UserModel _user;
        public ProfileViewModel(UserModel user)
        {
            _user = user;
            _user.UserDataChanged += () => User = _user.UserData;
            LogOutCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                _user.SignOut();
            });
        }

    }
}
