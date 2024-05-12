using QuartoService.Models;

namespace QuartoService.Data
{
    public interface IQuartoRepo
    {
        bool SaveChanges();

        IEnumerable<Quarto> GetAllQuartosForHotel(int hotelId);
        Quarto GetQuarto(int hotelId, int quartoId);
        void CreateQuarto(int hotelId, Quarto quarto);

        void CreateHotel(Hotel hotel);
        bool HotelExists(int hotelId);
    }
}