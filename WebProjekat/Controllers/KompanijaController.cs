using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KompanijaController : ControllerBase
    {
        public AerodromContext Context {get;set;}

        public KompanijaController(AerodromContext context)
        {
            Context=context;
        }

        [HttpGet]
        [Route("PreuzmiKompanijeZaAerodrom")]
       public async Task<ActionResult> PreuzmiKompanije()
       {
           try
            {
                 return Ok(await Context.Letovi.Include(d=>d.Destinacija).ThenInclude(k=>k.IDKompanije).Select(k => 
                 new { 
                     k.Destinacija.IDKompanije.ID, 
                     k.Destinacija.IDKompanije.ImeKompanije,
                      k.Destinacija.IDKompanije.BrSedista ,
                      k.Aerodrom
                    }).ToListAsync());
              
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }


    }
}
