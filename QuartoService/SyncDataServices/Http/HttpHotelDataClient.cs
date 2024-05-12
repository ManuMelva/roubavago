using System.Text;
using System.Text.Json;
using QuartoService.Data;
using QuartoService.Dtos;
using QuartoService.Models;

namespace QuartoService.SyncDataServices.Http
{
    public static class HttpHotelDataClient
    {
        public static async Task GetAllHotels(IApplicationBuilder app)
        {
            var _httpClient = new HttpClient();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _quartoRepo = serviceScope.ServiceProvider.GetService<IQuartoRepo>();
                var _configuration = serviceScope.ServiceProvider.GetService<IConfiguration>();

                try
                {
                    var response = await _httpClient.GetAsync($"{_configuration["HotelService"]}");

                    if (response.IsSuccessStatusCode)
                    {
                        var Hotels = JsonSerializer.Deserialize<List<Hotel>>(response.Content.ReadAsStringAsync().Result);

                        Hotels.ForEach(Hotel =>
                        {
                            _quartoRepo.CreateHotel(Hotel);
                            _quartoRepo.SaveChanges();
                        });
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}