namespace Assignment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Restaurants
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Description of Location")]
        public string Location { get; set; }

        [AllowHtml]
        [Display(Name = "Description of Restaurant")]
        public string Description { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUsersId { get; set; }

        [Range(1,9999999)]
        public int Seats { get; set; }
    }
}
