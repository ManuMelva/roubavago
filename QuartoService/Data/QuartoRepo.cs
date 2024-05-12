using QuartoService.Models;

namespace QuartoService.Data
{
    public class QuartoRepo : IQuartoRepo
    {
        private readonly AppDbContext _context;

        public QuartoRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateHotel(Hotel hotel)
        {
            ArgumentNullException.ThrowIfNull(nameof(hotel));

            hotel.ExternalId = hotel.id;

            _context.Hotels.Add(hotel);
        }

        public void CreateQuarto(int hotelId, Quarto quarto)
        {
            ArgumentNullException.ThrowIfNull(nameof(quarto));

            quarto.IdHotel = hotelId;
            _context.Quartos.Add(quarto);
        }

        public IEnumerable<Quarto> GetAllQuartos()
        {
            return _context.Quartos.ToList();
        }

        public IEnumerable<Quarto> GetAllQuartosForHotel(int hotelId)
        {
            return _context.Quartos
                .Where(q => q.IdHotel == hotelId)
                .OrderBy(q => q.Hotel.name);
        }

        public Quarto GetQuarto(int hotelId, int quartoId)
        {
            return _context.Quartos
                .Where(q => q.IdHotel == hotelId && q.Id == quartoId)
                .FirstOrDefault();
        }

        public bool HotelExists(int hotelId)
        {
            return _context.Hotels.Any(h => h.ExternalId == hotelId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}