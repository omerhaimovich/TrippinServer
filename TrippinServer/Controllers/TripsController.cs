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
            

            // Returns Trip
            return Ok();
        }



    }
}
