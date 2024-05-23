using DesktopApplication.Core;
using DesktopApplication.Core.Parser;
using DesktopApplication.MVVM.Model;
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
        public IReactiveCommand? HelpCommand { get; set; }
        #endregion


        private readonly UserModel _user;
        [Reactive] public bool IsAuth { get; private set; }

        public MainViewModel(AuthViewModel authVM, FriendsViewModel friendsVM, GroupsViewModel groupsVM,
            SearchViewModel searchVM, ProfileViewModel profileVM, UserModel user)
        {
            IsAuth = false;

            AuthVM = authVM;
            FriendsVM = friendsVM;
            GroupsVM = groupsVM;
            SearchVM = searchVM;
            ProfileVM = profileVM;

            _user = user;

            SetChromeCommands();
            SetViewTransitionCommands();

            CurrentView = AuthVM!;

           _user.SignedIn += () => { 
                IsAuth = true;
                CurrentView = ProfileVM!;
            };
            _user.SignedOut += () => { 
                IsAuth = false; 
                CurrentView = AuthVM;
            };
        }

        private void SetViewTransitionCommands()
        {
            IObservable<bool> isAuth = this.WhenAnyValue(
                x => x.IsAuth);
            ProfileCommand = ReactiveCommand.Create(() => CurrentView = IsAuth ? ProfileVM : AuthVM);
            FriendsCommand = ReactiveCommand.Create(() => {
                CurrentView = FriendsVM;
                FriendsVM.OnSelected?.Invoke();
            }, isAuth);
            GroupsCommand = ReactiveCommand.Create(() => {
                CurrentView = GroupsVM;
                GroupsVM.OnSelected?.Invoke();
            }, isAuth);
            SearchCommand = ReactiveCommand.Create(() =>
            {
                SearchVM.UpdateGroups();
                CurrentView = SearchVM;
            }, isAuth);

            HelpCommand = ReactiveCommand.Create(() =>
            {
                MessageBox.Show(
                    @"Для начала работы в программе необходимо авторизоваться через платформу Discord.
После чего во вкладке 'Группы' необходимо присоединиться к существующей группе или создать свою.

Внутри группы можно будет удалить кинопродукт из неё, поменять статус просмотра, а также удалить его.
                    
Для добавления кинопродукта в список группы необходимо зайти во вкладку 'Поиск', найти его через поиск и, выбрав в нижнем левом углу нужную группу, нажать кнопку 'Добавить'.
                    
                ");
            });
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