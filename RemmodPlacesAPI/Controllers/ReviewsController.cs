using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemodePlacesAPI.Data;

namespace RemmodPlacesAPI.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DataContext context;
       public ReviewsController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var reviews = await this.context.Reviews.ToListAsync();

            return Ok(reviews);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var reviews = await this.context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(reviews);
        }
      
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        
    }
}
