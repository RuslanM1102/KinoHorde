using DesktopApplication.Core.Database;
using DesktopApplication.MVVM.Model;
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
        private readonly Supabase.Client _client;
        private readonly UserModel _user;
        public ObservableCollection<Group> Groups { get; private set; }
        public IReactiveCommand? ClickCommand { get; set; }
        public Action<object> OnGroupClicked { get; set; }
        public GroupCollectionViewModel(Supabase.Client client, UserModel user)
        {
            _client = client;
            _user = user;

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

            User user = _user.UserData;
            var groupsID = userGroupsResponse.Models.Where(u => u.UserId == user.Id).Select(x => x.GroupId).ToList();

            Groups.Clear();
            foreach(var group in groupResponse.Models.Where(x => groupsID.Contains(x.Id)))
            { 
                Groups.Add(group);
            }
        }
    }
}
