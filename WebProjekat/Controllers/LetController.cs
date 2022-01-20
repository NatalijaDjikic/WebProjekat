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
    public class LetController : ControllerBase
    {
        public AerodromContext Context {get;set;}

        public LetController(AerodromContext context)
        {
            Context=context;
        }

        // ---------------------------PREUZIMANJE LETOVA----------------------------------
        [HttpGet]
        [Route("PreuzmiLetoveZaAerodrom")]
        public async Task<ActionResult> PreuzmiLetove()
        {
           try
            {
                 return Ok(await Context.Letovi.Include(ae=>ae.Aerodrom)
                 .Include(p=>p.Putnik)
                 .Include(d=>d.Destinacija).ThenInclude(k=>k.IDKompanije)
                 .Select(l => 
                 new { 
                        l.TipSedista,
                        l.Cena,
                        l.Putnik,
                        l.Destinacija,
                        l.Aerodrom
                    }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

        [HttpGet]
        [Route("PreuzmiLetoveZaAerodromSVE")]
        public async Task<ActionResult> PreuzmiLetoveSVE()
        {
           try
            {
                 return Ok(await Context.Letovi.Include(p=>p.Putnik)
                 .Include(d=>d.Destinacija)
                 .ThenInclude(k=>k.IDKompanije)
                 .Select(l => 
                 new { 
                        l.TipSedista,
                        l.Cena,
                        l.Putnik,
                        l.Destinacija,
                        l.Destinacija.IDKompanije
                    }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

        // -------------------------- DODAVANJE LETA ------------------------------------
        [EnableCors ("CORS")]
        [Route("DodatiLet/{tipSedista}/{cenaSedista}/{idPutnika}/{gradID}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult> DodajLet(string tipSedista,int cenaSedista,int idPutnika,int gradID)
        {
        try
            {
                var putnik= await Context.Putnici.FindAsync(idPutnika);
                var destinacija=await Context.Destinacije.FindAsync(gradID);
                Let l=new Let{
                    TipSedista=tipSedista,
                    Cena=cenaSedista,
                    Putnik=putnik,
                    Destinacija=destinacija
                };
                    Context.Letovi.Add(l);
                    await Context.SaveChangesAsync();
                return Ok($"Let je dodat! ID je: {l.ID}");
            }
            catch (Exception e)
                {
                return BadRequest(e.Message);
            }

        }


        [EnableCors("CORS")]
        [Route("DodatiLetFromBody/{jmbg}/{grad}/{datum}/{idAerodroma}")]
        [HttpPost]
        public async Task<ActionResult> DodajLetFromBody([FromBody] Let let, [FromRoute]string jmbg,[FromRoute] string grad,[FromRoute]string datum,int idAerodroma)
        {
            try
            {   
                var putnik= await Context.Putnici.Where(p=>p.JMBG==jmbg).FirstAsync();
                var destinacija=await Context.Destinacije.Where(d=> d.Grad==grad).Where(de=>de.DatumiVreme==datum).FirstOrDefaultAsync();
                var aerodrom=await Context.Aerodromi.FindAsync(idAerodroma);
                if(destinacija!=null)
                {
                    Let l=new Let{
                        TipSedista=let.TipSedista,
                        Cena=let.Cena,
                        Putnik=putnik,
                        Destinacija=destinacija,
                        Aerodrom=aerodrom
                    };
                    Context.Letovi.Add(l);
                    await Context.SaveChangesAsync();
                    return Ok($"Let je dodat! ID je: {l.ID}");
                }
                else return BadRequest("Ne postoji let za trazeni grad i datum!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

         [Route("IzbrisiLet/{jmbg}/{grad}/{datum}/{aID}")]
         [HttpDelete]
    public async Task<ActionResult> IzbrisatiLet(string jmbg, string grad, string datum,int aID)
        {
            try
            {
                if(jmbg!=null && jmbg.Length==13)
                {
                    var putnik = await Context.Putnici.Where(p=>p.JMBG==jmbg).FirstOrDefaultAsync();
                    var destinacija=await Context.Destinacije.Where(d=> d.Grad==grad).Where(de=>de.DatumiVreme==datum).FirstOrDefaultAsync();
                    if(putnik!=null && destinacija!=null)
                    {
                        var lett=await Context.Letovi.Where(p=> p.Putnik.ID == putnik.ID)
                                                    .Where(d=>d.Destinacija.ID==destinacija.ID)
                                                    .Where(ae=>ae.Aerodrom.ID==aID)
                                                    .FirstOrDefaultAsync();
                        
                        if(lett!=null)
                        { 
                            Context.Letovi.Remove(lett);
                            await Context.SaveChangesAsync();
                            return Ok($"Uspešno izbrisan let!");
                        }
                       else return BadRequest("Ne postoji let!");
                    }
                    else  
                    {return BadRequest("Ne postoji putnik sa tim jbmg-om ili ne postoji destinacija!");}
                }
                else {return BadRequest("JMBG mora imati 13 karaktera!");}
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}
