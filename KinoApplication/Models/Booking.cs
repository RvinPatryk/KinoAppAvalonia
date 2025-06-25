namespace KinoApplication.Models
{
    public class Booking
    {
        public string SeansKey { get; set; } = "";
        public int Row { get; set; }
        public int Column { get; set; }
        public string User { get; set; } = "";    // nowa właściwość
    }
}