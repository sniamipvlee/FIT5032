namespace Assignment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Reviews
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "User")]
        public string AspNetUsersId { get; set; }

        [Display(Name = "Restaurant Name")]
        public int RestaurantsId { get; set; }

        [AllowHtml]
        [Display(Name = "Description of Restaurant")]
        public string Description { get; set; }
    }
}
