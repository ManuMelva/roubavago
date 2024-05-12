using System.ComponentModel.DataAnnotations;

namespace QuartoService.Models
{
    public class Quarto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]  
        public int NumeroQuarto { get; set; }

        [Required]
        public int QuantidadeCamas { get; set; }

        public int IdHotel { get; set; }

        public Hotel Hotel { get; set; }
    }
}