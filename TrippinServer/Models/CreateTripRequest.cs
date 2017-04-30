using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippinServer.Models
{
    public class CreateTripRequest
    {
        public int Lat { get; set; }

        public int Lng { get; set; }

        public string UserEmail { get; set; }        
    }
}