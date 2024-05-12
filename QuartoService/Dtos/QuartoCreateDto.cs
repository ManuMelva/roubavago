using System.ComponentModel.DataAnnotations;

namespace QuartoService.Dtos
{
    public class QuartoCreateDto
    {
        [Required]  
        public int NumeroQuarto { get; set; }

        [Required]
        public int QuantidadeCamas { get; set; }
    }
}