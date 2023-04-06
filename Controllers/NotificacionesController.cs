using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using ms_notificaciones.Models;
namespace in_notificaciones.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificacionesController : ControllerBase
{
    [Route("correo")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreo(ModeloCorreo datos){
            var apiKey = Environment.GetEnvironmentVariable("app_inmobiliaria");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("paula.14canob@gmail.com", "paula cano");
            var subject = datos.asuntoCorreo;
            var to = new EmailAddress(datos.correoDestino, datos.nombreDestino);
            var plainTextContent = "plain text content";
            var htmlContent = datos.contenidoCorreo;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            if(response.StatusCode == System.Net.HttpStatusCode.Accepted){
                return Ok("correo enviado a la direccion" + datos.correoDestino);
            }else{
                return BadRequest("Error enviando el mensaje a la direccion: "+ datos.correoDestino);
            }

    }
}
