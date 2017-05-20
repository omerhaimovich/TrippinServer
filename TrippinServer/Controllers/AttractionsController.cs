using Algorithm.Attractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippinServer.Models.Attractions;

namespace TrippinServer.Controllers
{
    [RoutePrefix("api/attractions")]
    public class AttractionsController : ApiController
    {
        [HttpGet]
        [Route("GetAttractions")]
        // http://host:port/Users/GetTrip
        public IHttpActionResult GetAttractions(string tripId, double lat, double lng)
        {
            // Returns Trip
            return Ok(AttractionsBL.GetNewAttractions(tripId, lat, lng));
        }

        [HttpPost]
        [Route("AttractionChosen")]
        // http://host:port/Users/GetTrip
        public IHttpActionResult AttractionChosen([FromBody] AttractionChosenRequest objattChosen)
        {
            AttractionsBL.AttractionChosen(objattChosen.TripId, objattChosen.AttractionId);
            // Returns Trip
            return Ok();
        }

        [HttpPost]
        [Route("AttractionRated")]
        // http://host:port/Users/GetTrip
        public IHttpActionResult AttractionRated([FromBody] AttractionRatedRequest objattRated)
        {
            AttractionsBL.AttractionRated(objattRated.TripId, objattRated.AttractionId, objattRated.IsGoodAttraction);
            // Returns Trip
            return Ok();
        }
        

    }
}
