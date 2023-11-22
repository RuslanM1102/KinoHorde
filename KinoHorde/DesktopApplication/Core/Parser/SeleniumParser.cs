using DesktopApplication.MVVM.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopApplication.Core.Parser
{
    public class SeleniumParser : IDisposable
    {
        private ChromeDriver _driver;
        private ChromeOptions _options;
        private ChromeDriverService _service;
        private Task _initializition;
        public int FoundCount { get; private set; }
        public SeleniumParser()
        {
            _initializition = Task.Run(() =>
            {
                Initialize();
            });
        }

        public void Initialize()
        {
            if (_driver != null)
            {
                return;
            }
            _options = new ChromeOptions();
            _options.AddArguments("headless");
            _options.AddArgument("--window-size=1020,2080");
            _options.EnableMobileEmulation("Samsung Galaxy S8+");

            _service = ChromeDriverService.CreateDefaultService();
            _service.HideCommandPromptWindow = true;
            _driver = new ChromeDriver(_service, _options);
            _driver.Navigate().GoToUrl($"https://ru.kinorium.com");
        }

        private IWebElement? GetPage(string searchName, int page)
        {
            _driver.Navigate().GoToUrl($"https://ru.kinorium.com/search/?q={searchName}&page={page}&perpage=3&type=movie");
            try
            {
                FoundCount = int.Parse(Regex.Match(_driver.FindElement(By.ClassName("count")).GetAttribute("innerHTML"), @"\d+").Value);
                return _driver.FindElement(By.ClassName("filmList"));
            }
            catch
            {
                return null;
            }
        }

        public async Task<ObservableCollection<ParsedFilm>> SearchFilms(string searchName, int page = 1)
        {
            await _initializition;
            var films = new ObservableCollection<ParsedFilm>();
            var webPage = GetPage(searchName, page);

            if (webPage == null)
            {
                FoundCount = 0;
                return films;
            }

            foreach (var filmElement in webPage
                .FindElements(By.ClassName("filmList__item-content")))
            {
                var film = new ParsedFilm();
                film.ImageUrl = filmElement.FindElement(By.ClassName("posterWrap"))
                    .FindElement(By.ClassName("movie-list-poster"))
                    .GetAttribute("src");

                if (film.ImageUrl.Contains(".svg"))
                {
                    film.ImageUrl = "pack://application:,,,/../../Core/Styles/Icons/NoImage.png";
                }
                else
                {
                    film.ImageUrl = Regex.Replace(film.ImageUrl, @"movie/\d+/", "movie/300/");
                }

                var titleElement = filmElement.FindElement(By.ClassName("filmList__movie-name"))
                    .FindElement(By.TagName("i"));
                film.Id = int.Parse(titleElement.GetAttribute("data-id"));

                film.Title = titleElement.Text;
                film.OriginalTitle = filmElement.FindElement(By.ClassName("select-wrap")).Text;
                var info = filmElement.FindElement(By.ClassName("filmList__extra-info")).Text.Split("\r\n");
                film.ExtraInfo = info[0].ToUpper();
                var subinfo = info[1].Split(" • ");
                film.CountryInfo = subinfo[0];
                try
                {
                    film.Producer = subinfo[1];
                }
                catch { }

                films.Add(film);
            }
            return films;
        }


        public void Dispose()
        {
            _driver?.Dispose();
            _service?.Dispose();
        }
    }
}
