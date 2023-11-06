using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace KinopoiskParser
{
   

    public static class SeleniumParser
    {
        private static ChromeDriver _driver;
        private static ChromeOptions _options;
        private static ChromeDriverService _service;

        [ModuleInitializer]
        internal static void Initiliaze()
        {
            _options = new ChromeOptions();
            _options.AddArguments("headless");
            _options.AddArgument("--window-size=1020,2080");
            _options.EnableMobileEmulation("Samsung Galaxy S8+");

            _service = ChromeDriverService.CreateDefaultService();
            //_service.HideCommandPromptWindow = true;

            _driver = new ChromeDriver(_service,_options);
        }

        public static IWebElement GetPage()
        {
            _driver.Navigate().GoToUrl("https://ru.kinorium.com/search/?q=пила&page=1&perpage=5&type=movie");
            return _driver.FindElement(By.ClassName("filmList"));
        }
    }




    //public static class TestParser
    //{
    //    //текст запроса GraphQL без параметров
    //    //преобразуется в однострочный для корректной работы в качестве JSON-параметра
    //    private static readonly string basicQuery = string.Join(" ",@"
    //    query SuggestSearch($keyword: String!, $yandexCityId: Int, $limit: Int)
    //    {
    //        suggest(keyword: $keyword) {
    //            top(yandexCityId: $yandexCityId, limit: $limit) {
    //                topResult {
    //                    global {
    //                        ...SuggestMovieItem...SuggestPersonItem...SuggestCinemaItem...SuggestMovieListItem
    //                                                                                   __typename
    //                    }
    //                    __typename
    //                }
    //                movies {
    //                    movie {
    //                        ...SuggestMovieItem
    //                        __typename
    //                    }
    //                    __typename
    //                }
    //                persons {
    //                    person {
    //                        ...SuggestPersonItem
    //                        __typename
    //                    }
    //                    __typename
    //                }
    //                cinemas {
    //                    cinema {
    //                        ...SuggestCinemaItem
    //                        __typename
    //                    }
    //                    __typename
    //                }
    //                movieLists {
    //                    movieList {
    //                        ...SuggestMovieListItem
    //                        __typename
    //                    }
    //                    __typename
    //                }
    //                __typename
    //            }
    //            __typename
    //        }
    //    }
    //    fragment SuggestMovieItem on Movie
    //    {
    //        id
    //        contentId
    //        title {
    //            russian
    //            original
    //            __typename
    //        }
    //        rating {
    //            kinopoisk {
    //                isActive
    //                value
    //              __typename
    //            }
    //            __typename
    //        }
    //        poster
    //        {
    //            avatarsUrl
    //            fallbackUrl
    //          __typename
    //        }
    //        viewOption
    //        {
    //            buttonText
    //            isAvailableOnline: isWatchable(
    //              filter: { anyDevice: false, anyRegion: false }
    //            )
    //            purchasabilityStatus
    //            subscriptionPurchaseTag
    //            type
    //            availabilityAnnounce {
    //                groupPeriodType
    //                announcePromise
    //                availabilityDate
    //                type
    //            __typename
    //            }
    //            __typename
    //        }
    //    ... on Film
    //        {
    //            type
    //            productionYear
    //            __typename
    //        }
    //    ... on TvSeries
    //        {
    //            releaseYears {
    //                end
    //                start
    //              __typename
    //            }
    //            __typename
    //        }
    //    ... on TvShow
    //        {
    //            releaseYears {
    //                end
    //                start
    //              __typename
    //            }
    //            __typename
    //        }
    //    ... on MiniSeries
    //        {
    //            releaseYears {
    //                end
    //                start
    //              __typename
    //            }
    //            __typename
    //        }
    //        __typename
    //    }
    //    fragment SuggestPersonItem on Person
    //    {
    //      id
    //      name
    //      originalName
    //      birthDate
    //      poster {
    //            avatarsUrl
    //            fallbackUrl
    //            __typename
    //      }
    //        __typename
    //    }
    //    fragment SuggestCinemaItem on Cinema
    //    {
    //      id
    //      ctitle: title
    //      city {
    //            id
    //            name
    //            geoId
    //            __typename
    //      }
    //        __typename
    //    }
    //    fragment SuggestMovieListItem on MovieListMeta
    //    {
    //        id
    //        cover {
    //            avatarsUrl
    //            __typename
    //        }
    //        coverBackground {
    //            avatarsUrl
    //            __typename
    //        }
    //        name
    //        url
    //        description
    //        movies(limit: 0) {
    //            total
    //            __typename
    //        }
    //        __typename
    //    }".Split()
    //        .Where(x => !string.IsNullOrWhiteSpace(x)));

    //    public static async Task<string> GetMovies(string name)
    //    {
    //        var options = new RestClientOptions("https://graphql.kinopoisk.ru")
    //        {
    //            MaxTimeout = -1,
    //            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
    //        };
    //        var client = new RestClient(options);
    //        var request = new RestRequest("/graphql/?operationName=SuggestSearch", Method.Post);
    //        request.AddHeader("authority", "graphql.kinopoisk.ru");
    //        request.AddHeader("accept", "*/*");
    //        request.AddHeader("accept-language", "ru,en;q=0.9");
    //        request.AddHeader("content-type", "application/json");
    //        request.AddHeader("origin", "https://www.kinopoisk.ru");
    //        request.AddHeader("referer", "https://www.kinopoisk.ru/");
    //        request.AddHeader("service-id", "25");


    //        request.AddParameter(
    //            "application/json",
    //            BuildQuery(name, 213, 5),
    //            ParameterType.RequestBody);

    //        RestResponse response = await client.ExecuteAsync(request);
    //        return response.Content;

    //    }

    //    // добавление параметров к запросу
    //    public static string BuildQuery(string keyword, int cityId, int limit)
    //    {
    //       return $$"""
    //            {
    //                "query":"{{basicQuery}}",
    //                "variables":
    //                {
    //                    "keyword":"{{keyword}}",
    //                    "yandexCityId":{{cityId}},
    //                    "limit":{{limit}}
    //                }
    //            }
    //        """;
    //    }
    //}
}