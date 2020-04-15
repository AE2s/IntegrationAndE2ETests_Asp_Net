using System;
using System.ComponentModel.DataAnnotations;

namespace MVCAsyncAwait.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "please specify a name for the movie")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReleasedOn { get; set; }

        [Required]
        public GenreEnum Genre { get; set; }

        [Required]
        [StringLength(80,ErrorMessage = "writer name have more than 80 length")]
        public string Writer { get; set; }

    }
}