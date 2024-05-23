using DesktopApplication.Core.Database;
using DesktopApplication.MVVM.Model;
using Microsoft.Extensions.DependencyInjection;
using Postgrest;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Postgrest.QueryOptions;

namespace DesktopApplication.MVVM.ViewModel
{
    internal class GroupCreateViewModel
    {
        private readonly Supabase.Client _client;
        private readonly UserModel _user;
        public Action? OnCreated { get; set; }
        public string? Name { get; set; }
        public IReactiveCommand? CreateCommand { get; set; }

        public GroupCreateViewModel(Supabase.Client client, UserModel user)
        {
            _client = client;
            _user = user;
            CreateCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    Group group = new Group();
                    group.Name = Name;
                    var groupResponse = await _client.From<Group>().Insert(group, new QueryOptions { Returning = ReturnType.Representation });

                    var userGroup = new UserGroup();

                    userGroup.UserId = _user.UserData.Id;
                    userGroup.GroupId = groupResponse.Model.Id;
                    userGroup.IsOwner = true;

                    await _client.Postgrest.Table<UserGroup>().Insert(new List<UserGroup>() { userGroup });
                    OnCreated?.Invoke();
                }
                catch { }
            });
        }
    }
}
