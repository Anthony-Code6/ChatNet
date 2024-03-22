using ChatNet.Models.MensajeAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Proxy.RepositoryMensaje
{
    public interface IRepositoryMensaje
    {
        Task<int> EnviarMensaje(Mensajes mensajes);
    }
}
