using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Enums;

namespace TrippinServer.Models
{
    public class CreateTripRequest
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public string UserEmail { get; set; }

        public List<AttractionType> AttractionTypes { get; set; }
    }
}