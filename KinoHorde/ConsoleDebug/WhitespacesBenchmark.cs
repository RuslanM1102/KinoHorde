using System;
using BenchmarkDotNet.Attributes;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleDebug
{
    public partial class Program
    {
        public class WhitespacesBenchmark
        {
            public static string basicQuery = @"
        query SuggestSearch($keyword: String!, $yandexCityId: Int, $limit: Int)
        {
            suggest(keyword: $keyword) {
                top(yandexCityId: $yandexCityId, limit: $limit) {
                    topResult {
                        global {
                            ...SuggestMovieItem...SuggestPersonItem...SuggestCinemaItem...SuggestMovieListItem
                                                                                       __typename
                        }
                        __typename
                    }
                    movies {
                        movie {
                            ...SuggestMovieItem
                            __typename
                        }
                        __typename
                    }
                    persons {
                        person {
                            ...SuggestPersonItem
                            __typename
                        }
                        __typename
                    }
                    cinemas {
                        cinema {
                            ...SuggestCinemaItem
                            __typename
                        }
                        __typename
                    }
                    movieLists {
                        movieList {
                            ...SuggestMovieListItem
                            __typename
                        }
                        __typename
                    }
                    __typename
                }
                __typename
            }
        }
        fragment SuggestMovieItem on Movie
        {
            id
            contentId
            title {
                russian
                original
                __typename
            }
            rating {
                kinopoisk {
                    isActive
                    value
                  __typename
                }
                __typename
            }
            poster
            {
                avatarsUrl
                fallbackUrl
              __typename
            }
            viewOption
            {
                buttonText
                isAvailableOnline: isWatchable(
                  filter: { anyDevice: false, anyRegion: false }
                )
                purchasabilityStatus
                subscriptionPurchaseTag
                type
                availabilityAnnounce {
                    groupPeriodType
                    announcePromise
                    availabilityDate
                    type
                __typename
                }
                __typename
            }
        ... on Film
            {
                type
                productionYear
                __typename
            }
        ... on TvSeries
            {
                releaseYears {
                    end
                    start
                  __typename
                }
                __typename
            }
        ... on TvShow
            {
                releaseYears {
                    end
                    start
                  __typename
                }
                __typename
            }
        ... on MiniSeries
            {
                releaseYears {
                    end
                    start
                  __typename
                }
                __typename
            }
            __typename
        }
        fragment SuggestPersonItem on Person
        {
          id
          name
          originalName
          birthDate
          poster {
                avatarsUrl
                fallbackUrl
                __typename
          }
            __typename
        }
        fragment SuggestCinemaItem on Cinema
        {
          id
          ctitle: title
          city {
                id
                name
                geoId
                __typename
          }
            __typename
        }
        fragment SuggestMovieListItem on MovieListMeta
        {
            id
            cover {
                avatarsUrl
                __typename
            }
            coverBackground {
                avatarsUrl
                __typename
            }
            name
            url
            description
            movies(limit: 0) {
                total
                __typename
            }
            __typename
        }";

            public WhitespacesBenchmark()
            {
                
            }

            [Benchmark]
            public string LinqMethod()
            {
                return string.Join(" ",basicQuery
                    .Split()
                    .Where(x => !string.IsNullOrWhiteSpace(x)));
            }    
            
            [Benchmark]
            public string LinqMethod2()
            {
                return string.Join(" ", basicQuery.Split(" ",
                StringSplitOptions.RemoveEmptyEntries));
            }

            [Benchmark]
            public string RegexMethod()
            {
                return Regex.Replace(basicQuery, @"\s+", " ");
            }
        }
    }
}