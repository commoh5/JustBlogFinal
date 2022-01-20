using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JustBlog_Final.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title name is required.")]
        [StringLength(255)]
        [Display(Name = "Post title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Short Description is required.")]
        [StringLength(1024)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Content")]
        public string PostContent { get; set; }


        [StringLength(255)]
        [Display(Name = "Url")]
        public string UrlSlug { get; set; }

        [Display(Name = "Is Published")]
        public bool Published { get; set; }

        [Display(Name = "Posted On")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostedOn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Modified { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual IList<Tag> Tags { get; set; }

        public int? ViewCount { get; set; }
    }
}