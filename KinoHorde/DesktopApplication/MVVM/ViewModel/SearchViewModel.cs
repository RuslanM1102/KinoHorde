using DesktopApplication.Core.Database;
using DesktopApplication.Core.Parser;
using DesktopApplication.MVVM.Model;
using Microsoft.Extensions.DependencyInjection;
using Postgrest;
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
using static Postgrest.QueryOptions;

namespace DesktopApplication.MVVM.ViewModel
{
    class SearchViewModel : ReactiveObject
    {
        private readonly SeleniumParser _parser;
        private readonly Supabase.Client _client;
        private readonly UserModel _user;

        private int _maxPage;
        private string _search;
        [Reactive] public string? SearchName { get; set; }
        [Reactive] public int MovieCount { get; set; }
        [Reactive] public int CurrentPage { get; set; }
        [Reactive] public ObservableCollection<ParsedFilm>? Films { get; set; }
        [Reactive] public List<Group>? CurrentUserGroups { get; set; }
        public Group SelectedGroup { get; set; }
        public IReactiveCommand SearchCommand { get; set; }
        public IReactiveCommand NextPageCommand { get; set; }
        public IReactiveCommand PreviousPageCommand { get; set; }
        public IReactiveCommand ClickCommand { get; set; }

        public SearchViewModel(SeleniumParser parser, Supabase.Client client, UserModel user)
        {
            _parser = parser;
            _client = client;
            _user = user;
            SetCommands();
        }

        private void SetCommands()
        {
            SearchCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                _search = SearchName;
                Search();
            });

            this.WhenAnyValue(x => x.MovieCount).Subscribe(x =>
                {
                    _maxPage = MovieCount / 3;
                    if (MovieCount % 3 != 0)
                    {
                        _maxPage++;
                    }
                }
            );


            this.WhenAnyValue(x => x.CurrentPage).Subscribe(x =>
                {
                    Search(CurrentPage);
                }
            );

            IObservable<bool> canPrevious = this.WhenAnyValue(x => x.CurrentPage).Select(x => x > 1);
            PreviousPageCommand = ReactiveCommand.Create(() => CurrentPage--, canPrevious);

            IObservable<bool> canNext = this.WhenAnyValue(x => x.CurrentPage).Select(x => x < _maxPage); ;
            NextPageCommand = ReactiveCommand.Create(() => CurrentPage++, canNext);

            ClickCommand = ReactiveCommand.CreateFromTask<object>(async (args) =>
            {
                if (SelectedGroup == null)
                {
                    return;
                }
                else
                {
                    var parsed = (ParsedFilm)args;
                    var film = new Movie();
                    film.ImageUrl = parsed.ImageUrl;
                    film.Name = parsed.Title;
                    film.KinoriumId = parsed.Id.ToString();
                    var response = await _client.From<Movie>().Where(x => x.KinoriumId == film.KinoriumId).Get();
                    if (response.Model == null)
                    {
                        try
                        {
                            response = await _client.From<Movie>().Insert(film, new QueryOptions { Returning = ReturnType.Representation });
                        }
                        catch (Exception ex)
                        {
                            var a = ex.Message;
                        }
                    }
                    var movieGroup = new MovieGroups();
                    var userResponse = _user.UserData;

                    movieGroup.MovieId = response.Model.Id;
                    movieGroup.GroupId = SelectedGroup.Id;
                    movieGroup.UserId = userResponse.Id;
                    try
                    {
                        await _client.From<MovieGroups>().Insert(movieGroup);
                    }
                    catch (Exception ex)
                    {                    }
                    MessageBox.Show("Фильм добавлен");
                }
            });
        }

        private async Task Search(int page = 1)
        {
            Films = await _parser.SearchFilms(_search,page);
            MovieCount = _parser.FoundCount;
            CurrentPage = page;
        }

        public async void UpdateGroups()
        {
            User user = _user.UserData;

            var userGroups= await _client.From<UserGroup>()
                .Where(x => x.UserId == user.Id).Get();

            var groupsId = userGroups.Models.Select(x => x.GroupId);

            var groups = await _client.From<Group>().Get();

            CurrentUserGroups = groups.Models.Where(x => groupsId.Contains(x.Id)).ToList();
        }
    }
}
