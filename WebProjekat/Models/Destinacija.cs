using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Destinacija
    {
        [Key]
        public int ID {get;set;}

        [Required]
        [MaxLength(20)]
        public string Kontinent {get;set;}

        [Required]
        [MaxLength(20)]
        public string Drzava {get;set;}

        [Required]
        [MaxLength(20)]
        public string Grad {get;set;}

        [Required]
        public string DatumiVreme {get;set;}
        

       
        public Kompanija IDKompanije {get; set;}

        [JsonIgnore]
        public virtual List<Let> DestinacijaKompanija {get;set;}   //lista letova
    }

}


