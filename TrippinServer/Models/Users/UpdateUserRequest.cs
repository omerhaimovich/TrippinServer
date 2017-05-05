using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippinServer.Models.Users
{
    public class UpdateUserRequest
    {
        public string Email { get; set; }

        public bool NotificationsOn { get; set; }
    }
}