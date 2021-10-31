using System;


namespace MoviesVB.Core.Movies.Models
{
    public class MovieInfo
    {
        public Guid Id { get; set; }
        public string Search_Token { get; set; }
        public string ImdbID { get; set; }
        public double Processing_Time_Ms { get; set; }
        public string IP_Address { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
