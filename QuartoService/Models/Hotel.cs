using System.ComponentModel.DataAnnotations;

namespace QuartoService.Models
{
    public class Hotel
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public int ExternalId { get; set; }

        [Required]  
        public string name { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public string address { get; set; }

        public ICollection<Quarto> Quartos { get; set; } = new List<Quarto>();
    }
}