using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Models.MensajeAggregates
{
    public class Mensajes
    {
        public int? IdMensaje { get; set; } = 0;
        public int? IdEnvia { get; set; } 
        public int? IdResive { get; set; }
        public string? Mensaje { get; set; }
    }
}
