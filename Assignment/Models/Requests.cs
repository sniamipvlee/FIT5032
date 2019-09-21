namespace Assignment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Requests
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUsersId { get; set; }

        [Display(Name = "Restaurant Name")]
        public int RestaurantsId { get; set; }
    }
}
