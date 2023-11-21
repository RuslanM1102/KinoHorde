using DesktopApplication.Core.Database;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication.MVVM.ViewModel
{
    public class GroupCollectionViewModel
    {
        private Supabase.Client _client = ((App)App.Current).AppHost!.Services.GetRequiredService<Supabase.Client>();
        public ObservableCollection<Group> Groups { get; private set; }
        public IReactiveCommand? ClickCommand { get; set; }
        public Action<object> OnGroupClicked { get; set; }
        public GroupCollectionViewModel()
        {
            Groups = new ObservableCollection<Group>();

            ClickCommand = ReactiveCommand.Create<object>((args) => 
            { 
                OnGroupClicked?.Invoke(args);
            });
        }

        public async void SetGroups()
        {
            var groupResponse = await _client.From<Group>().Get();
            var userGroupsResponse = await _client.From<UserGroup>().Get();

            var response = await _client.From<User>().Where(x => x.IdSp == _client.Auth.CurrentUser!.Id).Get();
            User user = response.Model;

            var groupsID = userGroupsResponse.Models.Where(u => u.UserId == user.Id).Select(x => x.GroupId).ToList();

            Groups.Clear();
            foreach(var  group in groupResponse.Models.Where(x => groupsID.Contains(x.Id)))
            { 
                Groups.Add(group);
            }
        }
    }
}
