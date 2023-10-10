using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

// Supabase
namespace JwtWebApi.Models
{
    [Table("newsletters")]
    public class Newsletter : BaseModel
    {
        // set should insert to false - should auto generate id
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("read_time")]
        public int ReadTime { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}