using ChatNet.Models.MensajeAggregates;
using ChatNet.Proxy.RepositoryMensaje;
using ChatNet.Proxy.RepositoryUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatNet.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class ChatsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RepositoryMensaje _repositoryMensaje;
        private readonly RepositoryUsuario _repositoryUsuario;

        public ChatsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _repositoryUsuario = new RepositoryUsuario(_configuration);
            _repositoryMensaje = new RepositoryMensaje(_configuration);
        }

        [HttpGet]
        [Route("InformacionChats")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InformacionChats(int idusuario)
        {
            try
            {
                var usuario_logueado = await _repositoryUsuario.Usuario_GetUsuario(idusuario);
                var users = await _repositoryUsuario.Usuario_Lst(idusuario);

                return Ok(new { exito=true,mensajeError="", _usuario = usuario_logueado, _usuarios = users});
            }catch (Exception ex)
            {
                return BadRequest(new { exito=false, mensajeError= ex.Message });
            }
        }

        [HttpGet]
        [Route("MensajeAmigo")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> MensajeAmigo(int idusuario,int idamigo)
        {
            try
            {
                var usuario = await _repositoryUsuario.Usuario_GetUsuario(idamigo);
                var mensaje_amigo = await _repositoryUsuario.Mensaje_GetUsuario(idusuario,idamigo);

                return Ok(new { exito = true, mensajeError = "",_mensaje=mensaje_amigo, _usuario=usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, mensajeError = ex.Message });
            }
        }

        [HttpPost]
        [Route("SendMensaje")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendMensaje([FromBody] Mensajes mensajes)
        {
            try
            {
                int resulta = await _repositoryMensaje.EnviarMensaje(mensajes);

                return Ok(new { exito = true, mensajeError = "", _mensaje = resulta });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, mensajeError = ex.Message });
            }
        }

    }
}
