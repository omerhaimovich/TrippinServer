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
using TrippinServer.Models.Trips;

namespace TrippinServer.Controllers
{
    [RoutePrefix("api/trips")]
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

        [HttpGet]
        [Route("GetTrip")]       
        public IHttpActionResult GetTrip(string tripId, string userEmail, double lat, double lng)
        {
            return Ok(TripsBL.GetTrip(tripId, userEmail, lat, lng));
        }

        [HttpPost]
        [Route("UpdateTrip")]
        // http://host:port/Users/GetTrip
        public IHttpActionResult UpdateTrip([FromBody] UpdateTripRequest p_objTripUpdateRequest)
        {
            TripsBL.UpdateTrip(p_objTripUpdateRequest.TripId, p_objTripUpdateRequest.AttractionTypes);
            // Returns Trip
            return Ok();
        }


    }
}
