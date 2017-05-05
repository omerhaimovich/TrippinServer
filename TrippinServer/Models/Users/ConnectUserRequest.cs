using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippinServer.Models
{
    public class ConnectUserRequest
    {
        public string Email { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}