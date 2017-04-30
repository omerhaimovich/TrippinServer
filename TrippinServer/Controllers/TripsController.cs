using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using TrippinServer.Models;

namespace TrippinServer.Controllers
{
    public class TripsController : ApiController
    {
        [HttpPost]
        [Route("GetTrip")]
        // http://host:port/Users/GetTrip
        public IHttpActionResult GetTrip([FromBody] CreateTripRequest p_objTripCreationRequest)
        {
            String strCurrCountry = GMapsUtilities.GetCountryOfPoint(p_objTripCreationRequest.Lat, p_objTripCreationRequest.Lng);
            var objExistingTrip =  MongoAccess.Access<Trip>().FindSync<Trip>(objTrip => objTrip.Country == strCurrCountry && objTrip.UserEmail == p_objTripCreationRequest.UserEmail).FirstOrDefault();

            if(objExistingTrip == null)
            {
                objExistingTrip = new Trip()
                {
                    AwaitingAttractions = new List<Attraction>(),
                    AwaitingAttractionsIds = new List<string>(),
                    BadAttractions = new List<Attraction>(),
                    BadAttractionsIds = new List<string>(),
                    Country = strCurrCountry,
                    LovedAttractions = new List<Attraction>(),
                    LovedAttractionsIds = new List<string>(),
                    UserEmail = p_objTripCreationRequest.UserEmail,
                    Year = DateTime.Now.Year
                };

                MongoAccess.Access<Trip>().InsertOne(objExistingTrip);
            }

            // Returns Trip
            return Ok(objExistingTrip);
        }



    }
}
