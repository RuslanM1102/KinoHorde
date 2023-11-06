using DesktopApplication.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
        [Reactive] public object CurrentView { get; set; }
        public ProfileViewModel ProfileVM { get; set; }
        public FriendsViewModel FriendsVM { get; set; }
        public GroupsViewModel GroupsVM { get; set; }
        #endregion

        #region ChromeCommands
        public IReactiveCommand CloseCommand { get; set; }
        public IReactiveCommand MaximizeCommand { get; set; }
        public IReactiveCommand MinimizeCommand { get; set; }
        #endregion

        #region ViewTransitionCommands
        public IReactiveCommand ProfileCommand { get; set; }
        public IReactiveCommand FriendsCommand { get; set; }
        public IReactiveCommand GroupsCommand { get; set; }
        #endregion

        [Reactive] private bool IsAuth { get; set; }

        public MainViewModel()
        {
            IsAuth = false;

            SetChromeCommands();
            InitViewModels();
            SetViewTransitionCommands();

            CurrentView = ProfileVM;

            ProfileVM.AuthSuccessed += () => { IsAuth = true; };
        }

        private void SetViewTransitionCommands()
        {
            IObservable<bool> isAuth = this.WhenAnyValue(
                x => x.IsAuth);
            ProfileCommand = ReactiveCommand.Create(() => CurrentView = ProfileVM);
            FriendsCommand = ReactiveCommand.Create(() => CurrentView = FriendsVM, isAuth);
            GroupsCommand = ReactiveCommand.Create(() => CurrentView = GroupsVM, isAuth);
        }

        private void InitViewModels()
        {
            ProfileVM = new ProfileViewModel();
            FriendsVM = new FriendsViewModel();
            GroupsVM = new GroupsViewModel();
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
                Application.Current.Shutdown();
            });
        }
    }
}