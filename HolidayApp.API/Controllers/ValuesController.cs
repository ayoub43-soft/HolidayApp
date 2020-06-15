using System.Collections.Generic;
using System.Linq;
using HolidayApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HolidayApp.API.Controllers
{
    [Route("api/values")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        public ValuesController()
        {
            
        }

           
        [HttpGet("get_all_values")]
        [Authorize(Roles="Admin")] 
        public IActionResult GetValues()
        {
            var values = new List<Value>{
                new Value{Id= 34,Name = "value 1"},
                new Value{Id= 31,Name = "value 2"}
            };
            return Ok(values);
        }

        [HttpGet("{id}")]
        [Authorize(Roles="Member")]
        public IActionResult GetValue(int id)
        {
            var value = new Value{Id = 34,Name = "test value"};
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}