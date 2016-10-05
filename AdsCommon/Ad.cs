using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsCommon
{
    public enum Category
    {
        Cars,
        [Display(Name = "Real Estate")]
        RealEstate,
        [Display(Name = "Free Stuff")]
        FreeStuff
    }

    public class Ad
    {
        public int AdId { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public int Price { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(1000)]
        [DisplayName("Full-size Image")]
        public string ImageURL { get; set; }

        [StringLength(1000)]
        [DisplayName("Thumbnail")]
        public string ThumbnailURL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostedDate { get; set; }

        public Category? Category { get; set; }
        [StringLength(12)]
        public string Phone { get; set; }
    }
}
