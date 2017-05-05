using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippinServer.Models.Trips
{
    public class UpdateTripRequest
    {
        public string TripId { get; set; }

        public List<AttractionType> AttractionTypes { get; set; }
    }
}