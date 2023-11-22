using Postgrest.Attributes;
using Postgrest.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Core.Database
{
    [Table("users")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [PrimaryKey("sp_id")]
        public string IdSp { get; set; }
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
        [Column("password")]
        public string Password { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    [Table("users_groups")]
    public class UserGroup : BaseModel
    {
        [Column("user_id")]
        public int UserId { get; set; }
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
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
    }
}
