using DesktopApplication.Core.Database;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DesktopApplication.MVVM.ViewModel
{
    public class GroupViewModel
    {

        private Supabase.Client _client = ((App)App.Current).AppHost!.Services.GetRequiredService<Supabase.Client>();
        public ObservableCollection<Movie> Movies { get; set; }
        public Group Group { get; private set; }
        public GroupViewModel(Group group)
        {
            Movies = new ObservableCollection<Movie>();
            Group = group;
            SetMovies();
        }

        private async void SetMovies()
        {
            var response = await _client.From<Movie>().Get();
            Movies.Clear();
            foreach (var movie in response.Models)
            {
                Movies.Add(movie);
            }
        }
    }
}
