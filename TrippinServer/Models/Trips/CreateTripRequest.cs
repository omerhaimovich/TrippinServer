using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippinServer.Models
{
    public class CreateTripRequest
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public string UserEmail { get; set; }        
    }
}