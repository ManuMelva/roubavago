using System.ComponentModel.DataAnnotations;

namespace QuartoService.Dtos
{
    public class QuartoReadDto
    {
        public int Id { get; set; }

        public int NumeroQuarto { get; set; }

        public int QuantidadeCamas { get; set; }

        public int IdHotel { get; set; }
    }
}