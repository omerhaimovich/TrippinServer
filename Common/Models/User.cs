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
    public class User : ICollectional
    {
                
        [BsonElement("username")]
        public string Username { get; set; }

        [BsonId]
        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("trips")]
        public List<String> Trips { get; set; }

      

        [BsonElement("notificationsOn")]
        public bool NotificationsOn { get; set; }

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
