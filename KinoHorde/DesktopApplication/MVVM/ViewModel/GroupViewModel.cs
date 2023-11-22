using DesktopApplication.Core.Database;
using DesktopApplication.MVVM.Model;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace DesktopApplication.MVVM.ViewModel
{
    public class GroupViewModel
    {

        private Supabase.Client _client = ((App)App.Current).AppHost!.Services.GetRequiredService<Supabase.Client>();
        public ObservableCollection<MovieItem> Movies { get; set; }
        public Group Group { get; private set; }

        public IReactiveCommand StatusCommand { get; set; }
        public IReactiveCommand DeleteCommand { get; set; }
        public GroupViewModel(Group group)
        {
            Movies = new ObservableCollection<MovieItem>();
            Group = group;
            StatusCommand = ReactiveCommand.CreateFromTask<object>(async (arg) => 
            {
                if (arg is MovieItem item) 
                {
                    item.MovieGroups.StatusId = item.MovieGroups.StatusId == 1 ? 2:1;
                    item.Status = item.MovieGroups.StatusId == 1 ? "В планах" : "Просмотрено";
                     await _client.From<MovieGroups>()
                    .Where(x => x.GroupId == item.MovieGroups.GroupId)
                    .Where(x => x.MovieId == item.MovieGroups.MovieId)
                    .Set(x => x.StatusId, item.MovieGroups.StatusId)
                    .Update();

                    var i = Movies.IndexOf(item);
                    Movies.Remove(item);
                    Movies.Insert(i, item);
                }
            });

            DeleteCommand = ReactiveCommand.Create<object>(async (arg) =>
            {
                if (arg is MovieItem item)
                {
                    await _client.From<MovieGroups>()
                    .Where(x => x.GroupId == item.MovieGroups.GroupId)
                    .Where(x => x.MovieId == item.MovieGroups.MovieId)
                    .Delete();
                    Movies.Remove(item);
                }
            });

            SetMovies();
        }

        private async void SetMovies()
        {
            var response = await _client.From<Movie>().Get();

            var responseGroups= await _client.From<MovieGroups>().Where(x => x.GroupId == Group.Id).Get();

            var ids = responseGroups.Models.Select(x => x.MovieId).ToList();

            Movies.Clear();

            foreach (var movieGroup in responseGroups.Models)
            {
                var item = new MovieItem();
                item.MovieGroups = movieGroup;
                item.Status = item.MovieGroups.StatusId == 1 ? "В планах" : "Просмотрено";
                item.Movie = response.Models.Where(x=> x.Id == movieGroup.MovieId).First();
                Movies.Add(item);
            }
        }
    }
}
