using ChatNet.Models.MensajeAggregates;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatNet.Cliente.Controllers
{
    public class ChatsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ChatsController( IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("ServidorApi").GetSection("Url").Value);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Informacion()
        {
            var content = "";
            var response = await _httpClient.GetAsync("/api/Chats/InformacionChats?idusuario=1");
            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }
            return Json(content);
        }

        [HttpGet]
        public async Task<JsonResult> InformacionChat(int idamigo)
        {
            var content = "";
            var response = await _httpClient.GetAsync("/api/Chats/MensajeAmigo?idusuario=1&idamigo="+idamigo);
            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }
            return Json(content);
        }

        [HttpPost]
        public async Task<JsonResult> EnviarMensaje([FromBody] Mensajes mensajes)
        {
            var content = "";
            var mensaje = new StringContent(JsonConvert.SerializeObject(mensajes),Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Chats/SendMensaje", mensaje);

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }

            return Json(content);
        }
    }
}
