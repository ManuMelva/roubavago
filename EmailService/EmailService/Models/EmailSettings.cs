namespace EmailService.Models
{
    public class EmailSettings
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string HotelName { get; set; }
        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
        public int NumeroQuarto { get; set; }
        public string HotelEndereco { get; set; }
        public string HotelCidade { get; set; }
        public DateTime DataCancelamento { get; set; }
    }
}