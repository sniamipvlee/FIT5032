namespace Assignment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Requests
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUsersId { get; set; }

        public int RestaurantsId { get; set; }
    }
}
