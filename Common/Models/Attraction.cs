namespace Common.Models
{
    public class Attraction
    {
        public string ID { get; set; }
        public bool IsOpenNow { get; set; }
        public double Longitude { get; set; }
        public double latitude { get; set; }
        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string PhotoReference { get; set; }

        public float Rating { get; set; }
    }
}