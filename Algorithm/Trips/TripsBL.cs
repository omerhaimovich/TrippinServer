using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Common.Enums;
using MongoDB.Bson;

namespace Algorithm.Trips
{
    public class TripsBL
    {
        public static Trip CreateTrip(string p_strEmail, double p_dLat, double p_dLng, List<AttractionType> AttractionTypes)
        {
            User objUser = MongoAccess.Access<User>().FindSync(objCurrUser => objCurrUser.Email == p_strEmail).FirstOrDefault();

            if (objUser == null)
                throw new Exception();

            Trip objTrip = new Trip()
            {
                UserEmail = p_strEmail,
                Year = DateTime.Now.Year,
                UnratedAttractionsIds = new List<CoreAttraction>(),
                BadAttractions = new List<Attraction>(),
                BadAttractionsIds = new List<CoreAttraction>(),
                Country = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng),
                CreationDate = DateTime.Now,
                WantedAttractionsTypes = AttractionTypes,
                IsActive = true,
                GoodAttractions = new List<Attraction>(),
                GoodAttractionsIds = new List<CoreAttraction>(),
                UnratedAttractions = new List<Attraction>(),
            };
            
            MongoAccess.Access<Trip>().InsertOne(objTrip);
            
            return objTrip;
        }

        public static Trip GetTrip(string id, string p_strEmail, double p_dLat, double p_dLng)
        {
            Trip objTrip = MongoAccess.Access<Trip>().FindSync(objCurrTrip => objCurrTrip.Id == new MongoDB.Bson.ObjectId(id)).FirstOrDefault();

            string Country = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng);

            if (objTrip != null)
            {
                objTrip.GoodAttractions = new List<Attraction>();
                objTrip.BadAttractions = new List<Attraction>();
                objTrip.UnratedAttractions = new List<Attraction>();
                
                foreach (var attractionId in objTrip.GoodAttractionsIds)
                    objTrip.GoodAttractions.Add(GMapsUtilities.GetAttractionById(attractionId.Id));

                foreach (var attractionId in objTrip.BadAttractionsIds)
                    objTrip.BadAttractions.Add(GMapsUtilities.GetAttractionById(attractionId.Id));

                foreach (var attractionId in objTrip.UnratedAttractionsIds)
                    objTrip.UnratedAttractions.Add(GMapsUtilities.GetAttractionById(attractionId.Id));

                objTrip.IsActive = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng) == objTrip.Country;
            }

            return objTrip;
        }

        public static void UpdateTrip(string id, List<AttractionType> lstAttractionType)
        {
            MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(id), 
                new UpdateDefinitionBuilder<Trip>().Set(x => x.WantedAttractionsTypes, lstAttractionType));
        }

    }
}
