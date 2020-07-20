using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentForBoat.Models
{
    public class Boat
    {

        [DisplayName("Boat Number")]
        [Required(ErrorMessage = "Please enter boat number")]
        public string BoatNumber { get; set; }

        [DisplayName("Upload Image")]        
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase ImageFile { get; set; }

        [DisplayName("Boat Rate (Per Hours)")]
        [Required(ErrorMessage = "Please enter boat rate")]
        [Range(0.01, 999999999, ErrorMessage = "Price must be greater than 0.00")]
        public decimal BoatRate { get; set; }
    }
}