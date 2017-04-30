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

namespace TrippinServer.Controllers
{
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("CreateUser")]
        // http://host:port/Users/CreateUser
        public IHttpActionResult CreateUser([FromBody] CreateUserRequest p_objUserCreationRequest)
        {
            var objUserExist =  MongoAccess.Access<User>().FindSync<User>(objUser => objUser.Email == p_objUserCreationRequest.Email);

            if(objUserExist != null)
            {
                return BadRequest("User Already Exists");
            }

            User objNewUSer = new Common.Models.User()
            {
                Email = p_objUserCreationRequest.Email,
                Password = p_objUserCreationRequest.Password,
                Trips = new List<string>(),
                NotificationsOn = true,
                Attractions = new List<AttractionType>(Enum.GetValues(typeof(AttractionType)).Cast<AttractionType>()),
                Username = p_objUserCreationRequest.Name
            };

            MongoAccess.Access<User>().InsertOne(objNewUSer);

            // Returns User
            return Ok(objNewUSer);
        }

        [HttpPost]
        [Route("AuthenticateUser")]
        // http://host:port/Users/AuthenticateUser
        public IHttpActionResult AuthenticateUser([FromBody] AuthUserRequest p_objUserAuthRequest)
        {
            User objUserExist = MongoAccess.Access<User>().FindSync<User>(objUser => objUser.Email == p_objUserAuthRequest.Email &&
            objUser.Password == p_objUserAuthRequest.Password).FirstOrDefault();

            if (objUserExist == null)
            {
                return BadRequest("User does not exist");
            }

            objUserExist.Password = "";
            // Returns User
            return Ok(objUserExist);
        }

        // TODO: Limay
        [HttpPost]
        [Route("CreateOrAuthenticateUser")]
        // http://host:port/Users/CreateOrAuthenticateUser
        public IHttpActionResult CreateOrAuthenticateUser([FromBody] ConnectUserRequest p_objUserAuthRequest)
        {
            // Check to see if user exist by email if not create new one.
            return Ok();
        }

    }
}
