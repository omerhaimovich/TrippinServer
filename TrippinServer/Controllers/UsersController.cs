using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippinServer.Models;
using MongoDB.Driver;
using Common.Enums;
using Algorithm.Users;
using TrippinServer.Models.Users;

namespace TrippinServer.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        
        
        [HttpPost]
        [Route("ConnectUser")]
        // http://host:port/Users/ConnectUser
        public IHttpActionResult ConnectUser([FromBody] ConnectUserRequest p_objUserAuthRequest)
        {            
            // Check to see if user exist by email if not create new one.
            return Ok(UsersBL.GetUser(p_objUserAuthRequest.Email, p_objUserAuthRequest.Lat, p_objUserAuthRequest.Lng));
        }

        [HttpPost]
        [Route("UpdateUser")]
        // http://host:port/Users/ConnectUser
        public IHttpActionResult UpdateUser([FromBody] UpdateUserRequest p_objUserUpdateRequest)
        {

            UsersBL.UpdateUser(p_objUserUpdateRequest.Email.ToLower(), p_objUserUpdateRequest.NotificationsOn, p_objUserUpdateRequest.Radius);                        
            return Ok();
        }

    }
}
