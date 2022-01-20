using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;


namespace Models
{
    public class Kompanija 
    {
        [Key]
        public int ID {get;set;}

        [Required]
        [MaxLength(50)]
        public string ImeKompanije {get;set;}

        [Required]
        public int BrSedista{get;set;}

    

    }
}