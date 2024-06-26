﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Models.UsuarioAggregates
{
    public class Usuarios
    {
        public int? IdUsuario { get; set; } = 0;
        public string? Nombre { get; set; }
        public string? Apellido { get; set; } 
        public string? Correo {  get; set; }
        public string? Foto {  get; set; } = "";
        public string? Password {  get; set; }
    }
}
