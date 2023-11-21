using DesktopApplication.Core.Database;
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
        private Supabase.Client _client = ((App)App.Current).AppHost!.Services.GetRequiredService<Supabase.Client>();
        public Action? OnCreated { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public IReactiveCommand? CreateCommand { get; set; }

        public GroupCreateViewModel()
        {
            CreateCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    Group group = new Group();
                    group.Name = Name;
                    group.Password = Password;
                    var groupResponse = await _client.From<Group>().Insert(group, new QueryOptions { Returning = ReturnType.Representation });

                    var userGroup = new UserGroup();
                    var userResponse = await _client.From<User>().Where(x => x.IdSp == _client.Auth.CurrentUser!.Id).Get();

                    userGroup.UserId = userResponse.Model.Id;
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
