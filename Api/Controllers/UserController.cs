using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Courby.Core.Mail;

using Courby.Core.Data;
using Courby.Security;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // New: api/User/New
        public void New([FromBody]string email, [FromBody] string password)
        {
            Guid newId = Courby.Core.Data.User.CreateUser(email, password);

            if (newId != Guid.Empty)
            {
                // Send validation email. 
                //Courby.Core.Mail.Mailer.CreateMessage("VALIDATE_EMAIL","en-en", new Dictionary<string, string>() { })

            }
        }

        public void LoginUser([FromBody]string email, [FromBody] string password)
        {
            
        }

        [HttpGet("{id}", Name = "Get")]
        public void ValidateEmail(Guid id, [FromQuery]string email, [FromQuery] string password)
        {
            Courby.Core.Data.User.ConfirmEmail(id, email);

            // Redirect to More details page.
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {

            
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(Guid id)
        {
            return "value";
        }


        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] string value)
        {
        }
    }
}
