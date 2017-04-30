using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippinServer.Models;

namespace TrippinServer.Controllers
{
    public class TripsController : ApiController
    {
        [HttpPost]
        [Route("CreateTrip")]
        // http://host:port/Users/CreateUser
        public IHttpActionResult CreateUser([FromBody] CreateTripRequest p_objUserCreationRequest)
        {
            // Returns User
            return Ok();
        }

    }
}
