﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public partial class SP_GET_AERI_HEADERResult
    {
        public string NoContrato { get; set; }
        public string Direccion { get; set; }
        public string Arrendador { get; set; }
        public string Arrendatario { get; set; }
        public string Agente { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoInmueble { get; set; }
        public int Habitaciones { get; set; }
        public string FirmaArrendatario { get; set; }
        public string FirmaArrendador { get; set; }
    }
}
