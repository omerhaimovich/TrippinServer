using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippinServer.Models.Attractions
{
    public class EndAttractionRequest
    {
        public string TripId { get; set; }
        public string AttractionId { get; set; }
    }
}