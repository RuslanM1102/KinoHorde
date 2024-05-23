using DesktopApplication.Core.Database;
using DesktopApplication.MVVM.Model;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.MVVM.ViewModel
{
    class GroupsViewModel : ReactiveObject
    {

        private readonly Supabase.Client _client;
        private readonly UserModel _user;
        public Action OnSelected { get; set; }
        [Reactive] public object? CurrentView { get; set; }
        public GroupCollectionViewModel? CollectionView { get; set; }
        public GroupCreateViewModel? CreateView { get; set; }
        public GroupJoinViewModel? JoinView { get; set; }

        #region Commands
        public IReactiveCommand? CollectionCommand { get; set; }
        public IReactiveCommand? CreateCommand { get; set; }
        public IReactiveCommand? JoinCommand { get; set; }
        public IReactiveCommand LeaveCommand { get; set; }
        #endregion

        public GroupsViewModel(Supabase.Client client, UserModel user)
        {
            _client = client;
            _user = user;

            CollectionView = new GroupCollectionViewModel(client, user);
            CollectionView.OnGroupClicked += (args) => { CurrentView = new GroupViewModel((Group)args); };

            CreateView = new GroupCreateViewModel(client, user);
            CreateView.OnCreated += () => {
                CollectionView.SetGroups();
                CurrentView = CollectionView;
            };

            JoinView = new GroupJoinViewModel(_client, _user);
            JoinView.OnJoined += () => {
                CollectionView.SetGroups();
                CurrentView = CollectionView;
            };

            CollectionCommand = ReactiveCommand.Create(() =>
            {
                CollectionView.SetGroups();
                CurrentView = CollectionView;
            });

            CreateCommand = ReactiveCommand.Create(() => CurrentView = CreateView);
            JoinCommand = ReactiveCommand.Create(() => CurrentView = JoinView);

            CurrentView = CollectionView;
            
            OnSelected += () =>
            {
                CollectionView.SetGroups();
                CurrentView = CollectionView;
            };

            LeaveCommand = ReactiveCommand.Create<object>(async (arg) =>
            {
                if(arg is Group group)
                {
                    User user = _user.UserData;
                    await _client.From<UserGroup>()
                        .Where(x => x.UserId == user.Id)
                        .Where(x => x.GroupId == group.Id)
                        .Delete();
                }

                OnSelected?.Invoke();
            });
        }
    }
}
