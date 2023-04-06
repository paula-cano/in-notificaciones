using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using in_notificaciones.Models;
namespace ms_notificaciones.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificacionesController : ControllerBase
{
    [Route("correo-bienvenida")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreoBienvenida(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("app_inmobiliaria");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.MensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("TEST_TEMPLATE_ONE"));
        msg.SetTemplateData(new
        {
            name = datos.nombreDestino,
            message = "Bienvenido a la comunidad de la inmobiliaria."
        });
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a la dirección " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
        }
    }


    [Route("correo-recuperacion-clave")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreoRecuperacionClave(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("app_inmobiliaria");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.MensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("WELCOME_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new
        {
            name = datos.nombreDestino,
            message = "Esta es su nuevla clave... no la comparta."
        });
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a la dirección " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
        }
    }


    [Route("enviar-correo-2fa")]
    [HttpPost]
    public async Task<ActionResult> EnviarCorreo2fa(ModeloCorreo datos)
    {
        var apiKey = Environment.GetEnvironmentVariable("app_inmobiliaria");
        var client = new SendGridClient(apiKey);

        SendGridMessage msg = this.MensajeBase(datos);
        msg.SetTemplateId(Environment.GetEnvironmentVariable("TwoFA_SENDGRID_TEMPLATE_ID"));
        msg.SetTemplateData(new
        {
            nombre = datos.nombreDestino,
            mensaje = datos.contenidoCorreo,
            asunto = datos.asuntoCorreo
        });
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return Ok("Correo enviado a la dirección " + datos.correoDestino);
        }
        else
        {
            return BadRequest("Error enviando el mensaje a la dirección: " + datos.correoDestino);
        }
    }


    private SendGridMessage MensajeBase(ModeloCorreo datos)
    {
        var from = new EmailAddress(Environment.GetEnvironmentVariable("EMAIL_FROM"), Environment.GetEnvironmentVariable("NAME_FROM"));
        var subject = datos.asuntoCorreo;
        var to = new EmailAddress(datos.correoDestino, datos.nombreDestino);
        var plainTextContent = datos.contenidoCorreo;
        var htmlContent = datos.contenidoCorreo;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        return msg;
    }



}