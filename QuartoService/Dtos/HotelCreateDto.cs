using System.ComponentModel.DataAnnotations;

namespace QuartoService.Dtos
{
    public class HotelCreateDto
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public string address { get; set; }
    }
}