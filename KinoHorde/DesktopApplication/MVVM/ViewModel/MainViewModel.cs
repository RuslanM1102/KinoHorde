using DesktopApplication.Core;
using DesktopApplication.Core.Parser;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopApplication.MVVM.ViewModel
{
    class MainViewModel : ReactiveObject
    {
        #region Views
        [Reactive] public object? CurrentView { get; set; }
        public AuthViewModel? AuthVM { get; set; }
        public ProfileViewModel? ProfileVM { get; set; }
        public FriendsViewModel? FriendsVM { get; set; }
        public GroupsViewModel? GroupsVM { get; set; }
        public SearchViewModel? SearchVM { get; set; }
        #endregion

        #region ChromeCommands
        public IReactiveCommand? CloseCommand { get; set; }
        public IReactiveCommand? MaximizeCommand { get; set; }
        public IReactiveCommand? MinimizeCommand { get; set; }
        #endregion

        #region ViewTransitionCommands
        public IReactiveCommand? ProfileCommand { get; set; }
        public IReactiveCommand? FriendsCommand { get; set; }
        public IReactiveCommand? GroupsCommand { get; set; }
        public IReactiveCommand? SearchCommand { get; set; }
        #endregion

        [Reactive] public bool IsAuth { get; private set; }

        public MainViewModel()
        {
            IsAuth = false;

            SetChromeCommands();
            InitViewModels();
            SetViewTransitionCommands();

            CurrentView = AuthVM!;

            AuthVM!.AuthSuccessed += () => { 
                IsAuth = true;
                ProfileVM.SetUser();
                CurrentView = ProfileVM!;
            };
            ProfileVM!.LogOuted += () => { 
                IsAuth = false; CurrentView = AuthVM;
            };
        }

        private void SetViewTransitionCommands()
        {
            IObservable<bool> isAuth = this.WhenAnyValue(
                x => x.IsAuth);
            ProfileCommand = ReactiveCommand.Create(() => CurrentView = IsAuth ? ProfileVM : AuthVM);
            FriendsCommand = ReactiveCommand.Create(() => CurrentView = FriendsVM, isAuth);
            GroupsCommand = ReactiveCommand.Create(() => {
                CurrentView = GroupsVM;
                GroupsVM.OnSelected?.Invoke();
            }, isAuth);
            SearchCommand = ReactiveCommand.Create(() =>
            {
                SearchVM.UpdateGroups();
                CurrentView = SearchVM;
            }, isAuth);
        }

        private void InitViewModels()
        {
            AuthVM = new AuthViewModel();
            ProfileVM = new ProfileViewModel();
            FriendsVM = new FriendsViewModel();
            GroupsVM = new GroupsViewModel();
            SearchVM = new SearchViewModel();
        }

        private void SetChromeCommands()
        {
            CloseCommand = ReactiveCommand.Create(() =>
            {
                Application.Current.Shutdown();
            });

            MaximizeCommand = ReactiveCommand.Create(() =>
            {
                var current = Application.Current.MainWindow.WindowState;
                Application.Current.MainWindow.WindowState =
                    current == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            });

            MinimizeCommand = ReactiveCommand.Create(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }
    }
}