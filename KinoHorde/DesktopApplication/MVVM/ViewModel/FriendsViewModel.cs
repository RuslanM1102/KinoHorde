using DesktopApplication.Core.Database;
using DesktopApplication.MVVM.Model;
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

namespace DesktopApplication.MVVM.ViewModel
{
    class FriendsViewModel : ReactiveObject
    {
        private readonly Supabase.Client _client;
        private readonly UserModel _user;
        [Reactive] public User FindedFriend { get; set; } = new User();
        [Reactive] public User SelectedFriend { get; set; } = null;
        public ObservableCollection<User> FriendsProfiles { get; set; } = new();
        public ObservableCollection<User> FriendsRequests { get; set; } = new();
        public ObservableCollection<Friends> Friends { get; set; }

        [Reactive] public bool IsFriendFinded { get; private set; } = false;
        [Reactive] public List<Group>? CurrentUserGroups { get; set; }
        [Reactive] public string SearchString { get; set; } = default;
        [Reactive] public Group SelectedGroup { get; set; } = null;
        public IReactiveCommand SearchFriendsCommand { get; set; }
        public IReactiveCommand AddFriendCommand { get; set; }
        public IReactiveCommand AcceptFriendCommand { get; set; }
        public IReactiveCommand DeclineFriendCommand { get; set; }
        public IReactiveCommand DeleteFriendCommand { get; set; }
        public IReactiveCommand InviteFriendCommand { get; set; }
        public Action OnSelected { get; private set; }

        public FriendsViewModel(Supabase.Client client, UserModel user)
        {
            _client = client;
            _user = user;
            FindedFriend.Nickname = "Не найден";

            IObservable<bool> isFriendFinded = this.WhenAnyValue(
                x => x.IsFriendFinded);

            OnSelected += CleanSearch;
            OnSelected += UpdateFriends;
            OnSelected += UpdateGroups;

            SearchFriendsCommand = ReactiveCommand.Create(async () =>
            {
                try
                {
                    string trimmedId = SearchString.Trim();
                    if (trimmedId != _user.UserData.Id)
                    {
                        var response = await _client.From<User>().Where(x => x.Id == trimmedId).Get();
                        FindedFriend = response.Model;
                        IsFriendFinded = true;
                    }
                }
                catch (Exception ex)
                {
                    CleanSearch();
                }
            });

            AddFriendCommand = ReactiveCommand.Create(async () =>
            {
                try
                {
                    var friends = new Friends();
                    friends.UserId = _user.UserData.Id;
                    friends.FriendId = FindedFriend.Id;
                    friends.IsAccepted = false;
                    await _client.From<Friends>().Insert(friends);
                    MessageBox.Show("Заявка отправлена");
                }
                catch (Exception ex)
                {

                }
            }, isFriendFinded);

            AcceptFriendCommand = ReactiveCommand.Create<object>(async (args) =>
            {
                try
                {
                    if (args is User requester)
                    {
                        var friends = new Friends();
                        friends.UserId = _user.UserData.Id;
                        friends.FriendId = requester.Id;
                        friends.IsAccepted = true;
                        await _client.From<Friends>().Upsert(friends);

                        friends = new Friends();
                        friends.UserId = requester.Id;
                        friends.FriendId = _user.UserData.Id;
                        friends.IsAccepted = true;
                        await _client.From<Friends>().Upsert(friends);
                    }
                }
                catch (Exception ex)
                {

                }
                UpdateFriends();
            });

            DeclineFriendCommand = ReactiveCommand.Create<object>(async (args) =>
            {
                try
                {
                    if (args is User requester)
                    {
                        await _client.From<Friends>()
                        .Where(x => x.FriendId == _user.UserData.Id)
                        .Where(x => x.UserId == requester.Id)
                        .Delete();
                    }
                }
                catch (Exception ex)
                {

                }
                UpdateFriends();
            });

            DeleteFriendCommand = ReactiveCommand.Create<object>(async (args) =>
            {
                try
                {
                    if (args is User requester)
                    {
                        var friends = new Friends();
                        friends.UserId = _user.UserData.Id;
                        friends.FriendId = requester.Id;
                        friends.IsAccepted = true;
                        await _client.From<Friends>().Delete(friends);

                        friends = new Friends();
                        friends.UserId = requester.Id;
                        friends.FriendId = _user.UserData.Id;
                        friends.IsAccepted = true;
                        await _client.From<Friends>().Delete(friends);
                    }
                }
                catch (Exception ex)
                {

                }
                UpdateFriends();
            });

            InviteFriendCommand = ReactiveCommand.Create(async () =>
            {
                if (SelectedFriend != null && SelectedGroup != null)
                {
                    try
                    {
                        var userGroup = new UserGroup();
                        userGroup.UserId = SelectedFriend.Id;
                        userGroup.GroupId = SelectedGroup.Id;
                        userGroup.IsOwner = false;
                        await _client.Postgrest.Table<UserGroup>().Insert(new List<UserGroup>() { userGroup });
                        MessageBox.Show("Друг добавлен в группу");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }

        public async void UpdateFriends()
        {
            var responseFriends = await _client.From<Friends>().Where(x => x.FriendId == _user.UserData.Id).Get();
            FriendsRequests.Clear();
            FriendsProfiles.Clear();
            foreach (var request in responseFriends.Models.Where(x => x.IsAccepted == false))
            {
                try
                {
                    var profileResponse = await _client.From<User>().Where(x => x.Id == request.UserId).Get();
                    FriendsRequests.Add(profileResponse.Model);
                }
                catch (Exception ex)
                {
                }
            }

            foreach (var friend in responseFriends.Models.Where(x => x.IsAccepted == true))
            {
                try
                {
                    var profileResponse = await _client.From<User>().Where(x => x.Id == friend.UserId).Get();
                    FriendsProfiles.Add(profileResponse.Model);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void CleanSearch()
        {
            FindedFriend.BaseUrl = null;
            FindedFriend.ImageUrl = null;
            FindedFriend.Id = null;
            FindedFriend.Nickname = "Не найден";
            IsFriendFinded = false;
        }

        private async void UpdateGroups()
        {
            User user = _user.UserData;

            var userGroups = await _client.From<UserGroup>()
                .Where(x => x.UserId == user.Id).Get();

            var groupsId = userGroups.Models.Select(x => x.GroupId);

            var groups = await _client.From<Group>().Get();

            CurrentUserGroups = groups.Models.Where(x => groupsId.Contains(x.Id)).ToList();
        }
    }
}
