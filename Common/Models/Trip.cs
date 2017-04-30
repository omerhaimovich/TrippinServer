using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Trip
    {        
        [BsonElement("location")]
        public String Location { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("userEmail")]
        public string UserEmail { get; set; }

        [BsonElement("lovedAttractions")]
        public List<string> LovedAttractionsIds { get; set; }

        [BsonElement("badAttractions")]
        public List<string> BadAttractionsIds { get; set; }

        [BsonElement("awaitingdAttractions")]
        public List<string> AwaitingAttractionsIds { get; set; }


        [BsonIgnore]
        public List<Attraction> LovedAttractions { get; set; }

        [BsonIgnore]
        public List<Attraction> BadAttractions { get; set; }

        [BsonIgnore]
        public List<Attraction> AwaitingAttractions { get; set; }


    }
}
