using System;
using System.ComponentModel.DataAnnotations;

namespace Register.Models {

    [MetadataType(typeof(DeveloperMetaData))]
    public partial class Developer {
    }

    public class DeveloperMetaData {

        public int DeveloperId { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "First name should be less than 20 characters.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Last name should be less than 30 characters.")]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DayOfBirth { get; set; }
        [Range(0,50, ErrorMessage = "Range 0-50")]
        public int YearsExperience { get; set; }

    }
}