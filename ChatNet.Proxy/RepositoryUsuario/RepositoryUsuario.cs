using ChatNet.Models.MensajeAggregates;
using ChatNet.Models.UsuarioAggregates;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Proxy.RepositoryUsuario
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly IConfiguration _configuration;

        public RepositoryUsuario(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Mensajes>> Mensaje_GetUsuario(int idusuario, int idamigo)
        {
            List<Mensajes> list = new List<Mensajes>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQL")))
                {
                    using (SqlCommand comando = new SqlCommand("sp_Mensaje_GetUsuario", conexion))
                    {
                        conexion.Open();
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idusuario", idusuario));
                        comando.Parameters.Add(new SqlParameter("@idamigo", idamigo));

                        using (SqlDataReader reader = await comando.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Mensajes()
                                {
                                    IdMensaje = Convert.ToInt32(reader["IdMensaje"].ToString()),
                                    IdEnvia = Convert.ToInt32(reader["IdEnvia"].ToString()),
                                    IdResive = Convert.ToInt32(reader["IdResive"].ToString()),
                                    Mensaje = reader["Mensaje"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public async Task<Usuarios> Usuario_GetUsuario(int idusuario)
        {
            Usuarios usuarios = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQL")))
                {
                    using (SqlCommand comando = new SqlCommand("sp_Usuario_GetUsuario", conexion))
                    {
                        conexion.Open();
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idusuario", idusuario));

                        using (SqlDataReader reader = await comando.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    usuarios = new Usuarios()
                                    {
                                        IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                                        Nombre = reader["Nombre"].ToString(),
                                        Apellido = reader["Apellido"].ToString(),
                                        Correo = reader["Correo"].ToString(),
                                        Foto = reader["Foto"].ToString(),
                                        Password = reader["Password"].ToString()
                                    };
                                }
                            }
                        }
                    }
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return usuarios;
        }

        public async Task<List<Usuarios>> Usuario_Lst(int idusuario)
        {
            List<Usuarios> list = new List<Usuarios>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQL")))
                {
                    using (SqlCommand comando = new SqlCommand("sp_Usuario_Lst", conexion))
                    {
                        conexion.Open();
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@idusuario", idusuario));

                        using (SqlDataReader reader = await comando.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Usuarios()
                                {
                                    IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                                    Nombre = reader["Nombre"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Foto = reader["Foto"].ToString(),
                                    Password = reader["Password"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
    }
}
