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
    public class PutnikController : ControllerBase
    {
        public AerodromContext Context {get;set;}

        public PutnikController(AerodromContext context)
        {
            Context=context;
        }

    // ------------------------------------- PRIKAZ PUTNIKA ---------------------------------------
    [EnableCors("CORS")]
    [HttpGet]
    [Route("PrikaziPutnikeKojiPutuju/{datumiVreme}/{grad}/{idAerodroma}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult> PrikaziPutnike (string datumiVreme, string grad,int idAerodroma)
       {
           try
            {
                var put= Context.Letovi.Include(d=>d.Destinacija)
                                            .ThenInclude(k=>k.IDKompanije)
                                            .Include(p => p.Putnik)
                                            .Include(ae=>ae.Aerodrom)
                                            .Where(d=>d.Destinacija.Grad==grad && d.Destinacija.DatumiVreme==datumiVreme && d.Aerodrom.ID==idAerodroma);
                var putnici =await put.ToListAsync();   
                                    

                return Ok(
                    putnici.Select(p=>
                    new{
                        Ime=p.Putnik.Ime,
                        Prezime=p.Putnik.Prezime,
                        JMBG=p.Putnik.JMBG,
                        BrLicneKarte=p.Putnik.BrLicneKarte,
                        ImeKompanije=p.Destinacija.IDKompanije.ImeKompanije,
                        TipSedista=p.TipSedista,
                        Cena=p.Cena
                    }).ToList()
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

        
    [EnableCors("CORS")]
    [HttpGet]
    [Route("PreuzmiTipoveSedista")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult> PreuzmiTipoveSedistaUAvionu()
       {
           try
            {
                 return Ok(await Context.Letovi.Select(l => l.TipSedista ).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }

    [Route("PreuzmiPutnikeZaAerodrom")]
    [HttpGet]
    public async Task<ActionResult> PreuzmiPutnike()
    {
        try
        {
            var putnici=await Context.Letovi.Include(p=>p.Putnik).Select(d =>
                    new
                    { 
                        Ime=d.Putnik.Ime,
                        Prezime=d.Putnik.Prezime,
                        Email=d.Putnik.Email,
                        JMBG=d.Putnik.JMBG,
                        BrLicneKarte=d.Putnik.BrLicneKarte,
                        BrTelefona=d.Putnik.BrTelefona,
                        Aerodrom=d.Aerodrom
                    }).ToListAsync();

            return Ok(putnici);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //--------------------------------------- DODAVANJE PUTNIKA ----------------------------------------
    //Dodajemo putnika
    [EnableCors("CORS")]
    [Route("DodatiPutnika/{ime}/{prezime}/{email}/{jmbg}/{brojLicneKarte}/{brojTelefona}")]
    [HttpPost]
     public async Task <ActionResult> DodajPutnika (string ime,string prezime,string email,string jmbg,string brojLicneKarte,string brojTelefona)
     {
           if (string.IsNullOrWhiteSpace(ime) || ime.Length > 50)
            {
                return BadRequest("Pogrešno ime!");
            }

            if (string.IsNullOrWhiteSpace(prezime) || prezime.Length > 50)
            {
                return BadRequest("Pogrešno prezime!");
            }
             if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Morate upisati email!");
            }
            if(jmbg.Length != 13)
            {
                return BadRequest("JMBG mora imati 13 karaktera!");
            }
             if(brojLicneKarte.Length !=9)
            {
                return BadRequest("Broj licne karte mora imati 9 karaktera!");
            }
           
            if (string.IsNullOrWhiteSpace(brojTelefona))
            {
                return BadRequest("Telefon koji ste uneli je pogrešan");
            }
            
            try
            {
                 Putnik p = new Putnik
                {
                    Ime=ime,
                    Prezime=prezime,
                    Email=email,
                    JMBG=jmbg,
                    BrLicneKarte=brojLicneKarte,
                    BrTelefona=brojTelefona
                };

                Context.Putnici.Add(p);
                await Context.SaveChangesAsync();
                return Ok($"Putnik je dodat! ID je: {p.ID}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
     }

    [EnableCors("CORS")]
    [Route("DodatiPutnikaFromBody/{grad}/{datum}")]
    [HttpPost]
    public async Task<ActionResult> DodajPutnikaFromBody([FromBody] Putnik putnik,string grad, string datum)
    {
         try
        {
            var destinacija=await Context.Destinacije.Where(d=> d.Grad==grad).Where(de=>de.DatumiVreme==datum).FirstOrDefaultAsync();
            if(destinacija!=null)
            {
            if (string.IsNullOrWhiteSpace(putnik.Ime) || putnik.Ime.Length > 50)
            {
                return BadRequest("Pogrešno ime!");
            }

            if (string.IsNullOrWhiteSpace(putnik.Prezime) || putnik.Prezime.Length > 50)
            {
                return BadRequest("Pogrešno prezime!");
            }
                if (string.IsNullOrWhiteSpace(putnik.Email))
            {
                return BadRequest("Morate upisati email!");
            }
            if(putnik.JMBG.Length != 13)
            {
                return BadRequest("JMBG mora imati 13 karaktera!");
            }
                if(putnik.BrLicneKarte.Length !=9)
            {
                return BadRequest("Broj licne karte mora imati 9 karaktera!");
            }
            
            if (string.IsNullOrWhiteSpace(putnik.BrTelefona))
            {
                return BadRequest("Telefon koji ste uneli je pogrešan");
            }
            
                Context.Putnici.Add(putnik);
                await Context.SaveChangesAsync();
                return Ok($"Putnik je dodat! ID je: {putnik.ID}");
            }
            else return BadRequest("Ne postoji ova destiancija");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //preuzimanje putnika sa zadatim jmbg-om
    [EnableCors("CORS")]
    [HttpGet]
    [Route("PreuzmiPutnikaNaOsnovuJMBG/{jbmg}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult> PreuzmiPutnikaJMBG(string jmbg)
       {
           try
            {
                 return Ok(
                     await Context.Putnici.Where(p=> p.JMBG == jmbg)
                                           .Select(p=>p.ID)
                                           .FirstOrDefaultAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       }


    // ------------------------------------ IZMENI TIP SEDISTA -------------------------------------
    [Route ("IzmeniTipSedista/{tipSedista}/{brojLicneKarte}")]
    [HttpPut]
    public async Task<ActionResult> IzmeniSediste (string tipSedista,string brojLicneKarte)
{
            try
            {
                if(brojLicneKarte!=null && brojLicneKarte.Length==9)
                {
                    var putnik=Context.Letovi.Include(p=> p.Putnik)
                                        .Where(p=> p.Putnik.BrLicneKarte == brojLicneKarte).FirstOrDefault();
                    
                    if(putnik!=null)
                    { 
                        var cena=0;
                        if(tipSedista=="FC")
                            cena=50000;
                        else if(tipSedista == "EC")
                        cena= 10000;
                        else if(tipSedista == "BC")
                            cena=25000;

                        putnik.Cena=cena;
                        putnik.TipSedista=tipSedista;
                        Context.Letovi.Update(putnik);
                        await Context.SaveChangesAsync();
                        return Ok($"Uspešno izmenjen tip sedista!: {putnik.Putnik.BrLicneKarte}");
                    }
                    else
                    {
                        return BadRequest("Putnik sa zadatom licnom kartom ne postoji!");
                    }
                }
                else return BadRequest("Broj licne karte mora imati 9 karaktera!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

    }

    // ------------------------------------- BRISANJE PUTNIKA --------------------------------------
    [Route("IzbrisiPutnika/{jmbg}/{grad}/{datum}")]
    [HttpDelete]
    public async Task<ActionResult> IzbrisatiPutnika(string jmbg,string grad, string datum)
        {
            try
            {
                if(jmbg!=null && jmbg.Length==13)
                {
                    var putnik = await Context.Putnici.Where(p=>p.JMBG==jmbg).FirstOrDefaultAsync();

                    if(putnik!=null)
                    {
                        var letovi=await Context.Letovi.Where(p=> p.Putnik.ID == putnik.ID).ToListAsync();
                        
                        Context.Putnici.Remove(putnik);
                        letovi.ForEach(l=>
                        {
                            Context.Letovi.Remove(l);
                        });
                        await Context.SaveChangesAsync();
                        return Ok($"Uspešno izbrisan putnik: {putnik.JMBG}");
                    }
                    else  
                    {return BadRequest("Ne postoji putnik sa tim jbmg-om!");}
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



