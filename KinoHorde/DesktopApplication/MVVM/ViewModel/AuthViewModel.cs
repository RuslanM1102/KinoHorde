using DesktopApplication.MVVM.Model;
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
    class AuthViewModel
    {
        private readonly UserModel _user;
        public IReactiveCommand AuthCommand { get; set; }
        
        public AuthViewModel(UserModel user)
        {
            _user = user;
            AuthCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                _user.SignIn();
            }); ;
        }
    }
}
