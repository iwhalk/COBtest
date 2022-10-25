using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CajeroReceptor
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Delegación")]
        public string NumDelegacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Plaza de Cobro")]
        public string NumPlaza { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Turnos")]
        public string IdTurno { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Administrador")]
        public string NumGeaAdministrador { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Número de Cajero")]
        public string NumCapufeCajeroReceptor { get; set; }

        public int? IdBolsa { get; set; }
    }
}
