using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Common.Enums;

namespace Algorithm.Users
{
    public class UsersBL
    {
        /// <summary>
        /// Getting the user according to Email and if not exists creating new one.
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static User GetUser(string strEmail, double lat, double lng)
        {
            User objCurrUser = MongoAccess.Access<User>().FindSync(objUser => objUser.Email == strEmail).FirstOrDefault();
            
            if(objCurrUser == null)
            {
                objCurrUser = new Common.Models.User()
                {
                    Email = strEmail,                    
                    NotificationsOn = true,                    
                    ActiveTrip = null,
                    TripsObjects = new List<Trip>(),
                };

                MongoAccess.Access<User>().InsertOne(objCurrUser);
            }
            else
            {
                objCurrUser.TripsObjects = MongoAccess.Access<Trip>().Find(objTrip => objTrip.UserEmail == objCurrUser.Email).ToList();
                objCurrUser.ActiveTrip = objCurrUser.TripsObjects.Where(objTrip => objTrip.Country == GMapsUtilities.GetCountryOfPoint(lat, lng))
                                                                 .OrderByDescending(objTrip => objTrip.CreationDate).FirstOrDefault();
            }

            return objCurrUser;
        }

    }
}
