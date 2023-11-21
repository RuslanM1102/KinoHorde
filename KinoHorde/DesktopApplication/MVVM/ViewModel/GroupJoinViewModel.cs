using DesktopApplication.Core.Database;
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

        private Supabase.Client _client = ((App)App.Current).AppHost!.Services.GetRequiredService<Supabase.Client>();
        
        public string Id { get; set; }
        public string Password { get; set; }
        public IReactiveCommand? JoinCommand { get; set; }

        public Action OnJoined { get; set; }

        public GroupJoinViewModel()
        {
            
            JoinCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var id = int.Parse(Id);
                    var response = await _client.From<Group>().Where(x => x.Id == id).Get();
                    if (response.Model != null && response.Model.Password == Password)
                    {
                        var userGroup = new UserGroup();
                        var userResponse = await _client.From<User>().Where(x => x.IdSp == _client.Auth.CurrentUser!.Id).Get();

                        userGroup.UserId = userResponse.Model.Id;
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
