using System;
using Common.Interfaces;

namespace Common.Models
{

    public class Attraction : ICollectional
    {
        public string ID { get; set; }
        public bool IsOpenNow { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string PhotoReference { get; set; }

        public float Rating { get; set; }

        public string GetCollectionName()
        {
            return "Attractions";
        }
    }
}