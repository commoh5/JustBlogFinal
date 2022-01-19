using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustBlog_Final.Models
{
    public class Tag
    {
        [Key]
        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(255)]
        public string Name { get; set; }

        public int Count { get; set; }

        [StringLength(255)]
        public string UrlSlug { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public virtual IList<Post> Posts { get; set; }
    }
}