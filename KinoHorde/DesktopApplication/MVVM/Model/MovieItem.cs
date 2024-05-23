using DesktopApplication.Core.Database;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.MVVM.Model
{
    public class MovieItem
    {
        [Reactive] public Movie Movie { get; set; }
        [Reactive] public MovieGroups MovieGroups { get; set; }
        [Reactive] public WrappedEnum<MovieStatus> Status { get; set; }

        public List<WrappedEnum<MovieStatus>> AllStatuses { get; set; } = new List<WrappedEnum<MovieStatus>>
        {
            new WrappedEnum<MovieStatus>(MovieStatus.Planned),
            new WrappedEnum<MovieStatus>(MovieStatus.Watching),
            new WrappedEnum<MovieStatus>(MovieStatus.Paused),
            new WrappedEnum<MovieStatus>(MovieStatus.Done)
        };
    }
}
