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
using Algorithm.Trips;

namespace TrippinServer.Controllers
{
    public class TripsController : ApiController
    {
        [HttpPost]
        [Route("CreateTrip")]
        // http://host:port/Users/GetTrip
        public IHttpActionResult CreateTrip([FromBody] CreateTripRequest p_objTripCreationRequest)
        {
            // Returns Trip
            return Ok(TripsBL.CreateTrip(p_objTripCreationRequest.UserEmail, p_objTripCreationRequest.Lat, p_objTripCreationRequest.Lng));
        }

        //[HttpGet]
        //[Route("GetTrip")]
        // http://host:port/Users/GetTrip
        //public IHttpActionResult GetTrip([FromBody] CreateTripRequest p_objTripCreationRequest)
        //{
            // Returns Trip
            //return Ok(TripsBL.GetTrip(p_objTripCreationRequest.UserEmail, p_objTripCreationRequest.Lat, p_objTripCreationRequest.Lng));
        //}



    }
}
