using Common.Enums;
using Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [BsonIgnoreExtraElements]
    public class Trip : ICollectional
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("location")]
        public String Country { get; set; }

        [BsonElement("creationDate")]
        public DateTime CreationDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("userEmail")]
        public string UserEmail { get; set; }

        [BsonElement("goodAttractions")]
        public List<CoreAttraction> GoodAttractions { get; set; }

        [BsonElement("badAttractions")]
        public List<CoreAttraction> BadAttractions { get; set; }

        [BsonElement("unratedAttractions")]
        public List<CoreAttraction> UnratedAttractions { get; set; }

        [BsonElement("allAttractions")]
        public List<CoreAttraction> AllAttractions { get; set; }

        [BsonElement("wantedAttractions")]
        public List<AttractionType> WantedAttractionsTypes { get; set; }  

        [BsonIgnore]
        public bool IsActive { get; set; }


        public string GetCollectionName()
        {
            return "Trips";
        }
    }
}
