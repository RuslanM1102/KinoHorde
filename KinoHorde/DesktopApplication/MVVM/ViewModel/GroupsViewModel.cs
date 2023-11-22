using DesktopApplication.Core.Database;
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

        private Supabase.Client _client = ((App)App.Current).AppHost!.Services.GetRequiredService<Supabase.Client>();
        public Action OnSelected { get; set; }
        [Reactive] public object? CurrentView { get; set; }
        public GroupCollectionViewModel? CollectionView { get; set; }
        public GroupCreateViewModel? CreateView { get; set; }
        public GroupJoinViewModel? JoinView { get; set; }

        public IReactiveCommand? CollectionCommand { get; set; }
        public IReactiveCommand? CreateCommand { get; set; }
        public IReactiveCommand? JoinCommand { get; set; }
        public IReactiveCommand LeaveCommand { get; set; }


        public GroupsViewModel()
        {
            CollectionView = new GroupCollectionViewModel();
            CollectionView.OnGroupClicked += (args) => { CurrentView = new GroupViewModel((Group)args); };

            CreateView = new GroupCreateViewModel();
            CreateView.OnCreated += () => {
                CollectionView.SetGroups();
                CurrentView = CollectionView;
            };

            JoinView = new GroupJoinViewModel();
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
                    var response = await _client.From<User>().Where(x => x.IdSp == _client.Auth.CurrentUser!.Id).Get();
                    User user = response.Model;

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
