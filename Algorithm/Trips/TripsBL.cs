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
                UnratedAttractions = new List<CoreAttraction>(),                
                BadAttractions = new List<CoreAttraction>(),
                Country = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng),
                CreationDate = DateTime.Now,
                WantedAttractionsTypes = AttractionTypes,
                IsActive = true,                
                GoodAttractions = new List<CoreAttraction>(),                 
            };
            
            MongoAccess.Access<Trip>().InsertOne(objTrip);
            
            return objTrip;
        }

        public static ExtendedTrip GetTrip(string id, string p_strEmail, double p_dLat, double p_dLng)
        {
            Trip objTrip = MongoAccess.Access<Trip>().FindSync(objCurrTrip => objCurrTrip.Id == new MongoDB.Bson.ObjectId(id)).FirstOrDefault();
           
            string Country = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng);

            if (objTrip != null)
            {
                ExtendedTrip objFullTrip = new ExtendedTrip(objTrip);

                var GoodAttractions = new List<Attraction>();
                var BadAttractions = new List<Attraction>();
                var UnratedAttractions = new List<Attraction>();
                var AllAttractions = new List<Attraction>();
                foreach (var attractionId in objTrip.GoodAttractions.Where(x => x != null))
                {
                    if (attractionId != null)
                    {
                        Attraction att = GMapsUtilities.GetAttractionByCore(attractionId);
                        GoodAttractions.Add(att);
                        AllAttractions.Add(att);
                    }
                }

                foreach (var attractionId in objTrip.BadAttractions.Where(x => x != null))
                {
                    Attraction att = GMapsUtilities.GetAttractionByCore(attractionId);
                    BadAttractions.Add(att);
                    AllAttractions.Add(att);
                }

                foreach (var attractionId in objTrip.UnratedAttractions.Where(x => x != null))
                {
                    Attraction att = GMapsUtilities.GetAttractionByCore(attractionId);
                    UnratedAttractions.Add(att);
                    AllAttractions.Add(att);
                }

                objFullTrip.IsActive = GMapsUtilities.GetCountryOfPoint(p_dLat, p_dLng) == objTrip.Country;
                objFullTrip.FullGoodAttractions= GoodAttractions;
                objFullTrip.FullBadAttractions = BadAttractions;
                objFullTrip.FullUnratedAttractions  = UnratedAttractions;
                objFullTrip.FullAllAttractions = AllAttractions;

                return objFullTrip;
            }

            return null;
        }

        public static void UpdateTrip(string id, List<AttractionType> lstAttractionType)
        {
            MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(id), 
                new UpdateDefinitionBuilder<Trip>().Set(x => x.WantedAttractionsTypes, lstAttractionType));
        }

        public static void EndTrip(string id)
        {
            MongoAccess.Access<Trip>().FindOneAndUpdate(objTrip => objTrip.Id == new ObjectId(id),
                new UpdateDefinitionBuilder<Trip>().Set(x => x.EndDate, DateTime.Now));
        }
    }
}
