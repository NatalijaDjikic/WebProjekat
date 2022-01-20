using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AerodromController : ControllerBase
    {
        public AerodromContext Context {get;set;}

        public AerodromController(AerodromContext context)
        {
            Context=context;
        }

    
        [HttpGet]
        [Route("PreuzmiAerodrome")]
        public async Task<ActionResult> PreuzmiAerodrome()
        {
           try
            {
                 return Ok(await Context.Aerodromi.ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }


    }

}
