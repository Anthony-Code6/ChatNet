using ChatNet.Models.MensajeAggregates;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Proxy.RepositoryMensaje
{
    public class RepositoryMensaje : IRepositoryMensaje
    {
        private readonly IConfiguration _configuration;

        public RepositoryMensaje(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> EnviarMensaje(Mensajes mensajes)
        {
            int respuesta = 0;
            try
            {
                using (SqlConnection conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQL")))
                {
                    using (SqlCommand comando = new SqlCommand("sp_EnviarMensaje", conexion))
                    {
                        conexion.Open();
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idenvia", mensajes.IdEnvia));
                        comando.Parameters.Add(new SqlParameter("@idrecibe", mensajes.IdResive));
                        comando.Parameters.Add(new SqlParameter("@mensaje", mensajes.Mensaje));

                        await comando.ExecuteNonQueryAsync();

                        respuesta = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                respuesta = 0 ;
            }
            return respuesta;
        }
    }
}
