using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Common.Enums;

namespace Algorithm.Trips
{
    public class TripsBL
    {
        public static Trip CreateTrip(string p_strEmail, double p_dLat, double p_dLng)
        {
            Trip objTrip = new Trip()
            {
                UserEmail = p_strEmail,
                Year = DateTime.Now.Year,
                UnratedAttractionsIds = new List<string>(),
                BadAttractions = new List<Attraction>(),
                BadAttractionsIds = new List<string>(),
                Country = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng),
                CreationDate = DateTime.Now,
                WantedAttractionsTypes = new List<AttractionType>(Enum.GetValues(typeof(AttractionType)).Cast<AttractionType>()),
                IsActive = true,
                GoodAttractions = new List<Attraction>(),
                GoodAttractionsIds = new List<string>(),
                UnratedAttractions = new List<Attraction>(),
            };

            MongoAccess.Access<Trip>().InsertOne(objTrip);

            User objUser = MongoAccess.Access<User>().FindSync(objCurrUser => objCurrUser.Email == p_strEmail).FirstOrDefault();

            if (objUser == null)
                throw new Exception();
            
            MongoAccess.Access<User>().FindOneAndUpdate(objCurrUser => objCurrUser.Email == p_strEmail, 
                                      new MongoDB.Driver.UpdateDefinitionBuilder<User>().AddToSet(x => x.Trips, objTrip.Id.ToString()));

            return objTrip;
        }

        public static Trip GetTrip(string id, string p_strEmail, double p_dLat, double p_dLng)
        {
            Trip objTrip = MongoAccess.Access<Trip>().FindSync(objCurrTrip => objCurrTrip.Id == new MongoDB.Bson.ObjectId(id)).FirstOrDefault();

            string Country = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng);

            objTrip.GoodAttractions = new List<Attraction>();
            objTrip.BadAttractions = new List<Attraction>();
            objTrip.UnratedAttractions = new List<Attraction>();

            foreach (var attractionId in objTrip.GoodAttractionsIds)            
                objTrip.GoodAttractions.Add(GMapsUtilities.GetAttractionById(attractionId));

            foreach (var attractionId in objTrip.BadAttractionsIds)
                objTrip.BadAttractions.Add(GMapsUtilities.GetAttractionById(attractionId));

            foreach (var attractionId in objTrip.UnratedAttractionsIds)
                objTrip.UnratedAttractions.Add(GMapsUtilities.GetAttractionById(attractionId));

            objTrip.IsActive = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng) == objTrip.Country;

            return objTrip;
        }

    }
}
