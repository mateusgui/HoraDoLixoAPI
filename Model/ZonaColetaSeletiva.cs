using System.ComponentModel.DataAnnotations;

namespace HoraDoLixo.Model
{
    public class ZonaColetaSeletiva
    {
        public ZonaColetaSeletiva()
        {
                
        }
        public int IdColetaSeletiva { get; set; }
        [Required]
        [StringLength(150)]
        public string NomeZonaSeletiva { get; set; }
    }
}
