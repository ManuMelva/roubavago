using AutoMapper;
using QuartoService.Dtos;
using QuartoService.Models;

namespace QuartoService.Profiles
{
    public class QuartosProfile: Profile
    {
        public QuartosProfile()
        {
            CreateMap<Quarto, QuartoReadDto>();
            CreateMap<QuartoCreateDto, Quarto>();
            CreateMap<Hotel, HotelReadDto>();
            CreateMap<HotelCreateDto, Hotel>();
        }
    }
}