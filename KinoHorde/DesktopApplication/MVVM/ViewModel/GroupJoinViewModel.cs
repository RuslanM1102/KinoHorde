using DesktopApplication.Core.Database;
using DesktopApplication.MVVM.Model;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.MVVM.ViewModel
{
    public class GroupJoinViewModel
    {

        private readonly Supabase.Client _client;
        private readonly UserModel _user;
        
        public string Id { get; set; }
        public string Password { get; set; }
        public IReactiveCommand? JoinCommand { get; set; }

        public Action OnJoined { get; set; }

        public GroupJoinViewModel(Supabase.Client client, UserModel user)
        {
            _client = client;
            _user = user;
            JoinCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var id = int.Parse(Id);
                    var response = await _client.From<Group>().Where(x => x.Id == id).Get();
                    if (response.Model != null)
                    {
                        var userGroup = new UserGroup();
                        userGroup.UserId = _user.UserData.Id;
                        userGroup.GroupId = response.Model.Id;
                        userGroup.IsOwner = false;
                        await _client.Postgrest.Table<UserGroup>().Insert(new List<UserGroup>() { userGroup});
                        OnJoined?.Invoke();
                    }
                }
                catch(Exception) { }
            });
        }
    }
}
