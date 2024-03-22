using ChatNet.Models.MensajeAggregates;
using ChatNet.Models.UsuarioAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Proxy.RepositoryUsuario
{
    public interface IRepositoryUsuario
    {
        Task<Usuarios> Usuario_GetUsuario(int idusuario);
        Task<List<Mensajes>> Mensaje_GetUsuario(int idusuario, int idamigo);
        Task<List<Usuarios>> Usuario_Lst(int idusuario);
    }
}
