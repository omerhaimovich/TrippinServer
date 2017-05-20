using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Common.Interfaces;
using Common.Enums;

namespace Common.Models
{
    [BsonIgnoreExtraElements]
    public class User : ICollectional
    {
                
        [BsonId]
        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("notificationsOn")]
        public bool NotificationsOn { get; set; }

        [BsonElement("radius")]
        public int Radius { get; set; }

        [BsonIgnore]
        public List<Trip> TripsObjects { get; set; }

        [BsonIgnore]
        public Trip ActiveTrip { get; set; }


        public string GetCollectionName()
        {
            return "Users";
        }
    }
}
