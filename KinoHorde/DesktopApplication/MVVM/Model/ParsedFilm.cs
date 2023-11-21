using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DesktopApplication.MVVM.Model
{
    public class ParsedFilm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string ExtraInfo { get; set; }
        public string CountryInfo { get; set; }
        public string Producer { get; set; }
        public string ImageUrl { get; set; }
        public string RatingIMDB { get; set; }
    }
}
