using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;


namespace Models{
    
    public class Aerodrom
    {
        [Key]
        public int ID{get;set;}
        
        [Required]
        public string ImeAerodroma {get;set;}  

        [Required]
        public string ImeGrada{get;set;}  

        [JsonIgnore]
        public virtual List <Let> LetoviZaAerodrom{get;set;} //lista letova
    }
}