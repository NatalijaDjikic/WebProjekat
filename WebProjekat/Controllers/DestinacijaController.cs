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
    public class DestinacijaController : ControllerBase
    {
        public AerodromContext Context {get;set;}

        public DestinacijaController(AerodromContext context)
        {
            Context=context;
        }

       
        [Route("PreuzmiDestinacijeZaAerodrom")]
        [HttpGet]
       public async Task<ActionResult> PreuzmiDestinacije()
       {
           try
            {
                // var dest= await Context.Destinacije.Include(d=>d.DestinacijaKompanija).Select(d=>new{
                //             ID=d.ID,
                //             Kontinent=d.Kontinent,
                //             Drzava=d.Drzava,
                //             Grad=d.Grad,
                //             DatumiVreme=d.DatumiVreme
                // }).ToListAsync();
                var dest=await Context.Letovi.Include(d=>d.Destinacija).Select(d =>
                        new
                        {
                            ID=d.Destinacija.ID,
                            Kontinent=d.Destinacija.Kontinent,
                            Drzava=d.Destinacija.Drzava,
                            Grad=d.Destinacija.Grad,
                            DatumiVreme=d.Destinacija.DatumiVreme,
                            Aerodrom=d.Aerodrom
                        }).ToListAsync();
                    
                return Ok(dest);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }



}
}
