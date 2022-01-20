using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;

namespace Models{
    [Table ("Putnik")]
    public class Putnik
    {
        [Key]
        public int ID {get;set;}

        [Required]
        [MaxLength(50)]
        public string Ime {get;set;}

        [Required]
        [MaxLength(50)]
        public string Prezime {get;set;}

        [Required]
        [MaxLength(55)]
        public string Email{get;set;}

        [Required]  
        [MaxLength(13)]
        public string JMBG {get;set;}

        [Required]
        [MaxLength(9)]
        public string BrLicneKarte{get;set;}

        [Required]
        [MaxLength(10)]
         public string BrTelefona{get;set;}

        [JsonIgnore]
        public virtual List <Let> PutnikLet{get;set;} //putnik ima listu letova
    }
}