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
        [Reactive] public string Status { get; set; }
    }
}
