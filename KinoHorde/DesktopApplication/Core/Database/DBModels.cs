using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Postgrest.Attributes;
using Postgrest.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Core.Database
{
    [Table("users")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; }
        [Column("nickname")]
        public string Nickname { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
    }

    [Table("groups")]
    public class Group : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    [Table("users_groups")]
    public class UserGroup : BaseModel
    {
        [Column("user_id")]
        public string UserId { get; set; }
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("is_owner")]
        public bool IsOwner { get; set; }
        [Column("nickname")]
        public string Nickname { get; set; }
    }

    [Table("movies")]
    public class Movie : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("kinorium_id")]
        public string KinoriumId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("image_url")]
        public string? ImageUrl { get; set; }
        [Column("is_series")]
        public bool? IsSeries { get; set; }
        [Column("episodes_quantity")]
        public int? EpisodeQuantity { get; set; }
    }

    [Table("movies_groups")]
    public class MovieGroups : BaseModel
    {
        [Column("movie_id")]
        public int MovieId { get; set; }
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("status")]
        public MovieStatus Status { get; set; }
        [Column("user_id")]
        public string UserId { get; set; }
    }

    [Table("friends")]
    public class Friends : BaseModel
    {
        [PrimaryKey("user_id", shouldInsert:true)]
        public string UserId { get; set; }
        [PrimaryKey("friend_id", shouldInsert: true)]
        public string FriendId { get; set; }
        [Column("is_accepted")]
        public bool IsAccepted { get; set; }
    }
}
