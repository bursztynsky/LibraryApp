using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Models
{
    public class Category
    {
        // Dzieki temu atrybutowi EF wie ze ID to jego klucz. Domyslnie sam znajdzie to co nazywa sie Id i bedzie kluczem
        [Key]
        public int Id { get; set; }

        // Informuje EF, ze to pole nie moze byc puste
        [Required]
        public string Name { get; set; }

        // Mozemy ustawic jaka nazwa ma byc wyswietlona korzystajac z label asp-for oraz w walidacji
        [DisplayName("Created Date")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
