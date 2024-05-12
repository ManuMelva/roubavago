using System.Text;
using EmailService.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EmailService.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmailsController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(EmailSettings emailSetting)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_configuration["EmailSettings:FromName"], _configuration["EmailSettings:FromEmail"]));
                email.To.Add(new MailboxAddress(emailSetting.Email, emailSetting.Email));
                email.Subject = "Reserva do Quarto Tal";
                email.Priority = MessagePriority.Urgent;

                StringBuilder sb = new();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang=\"pt-br\">");
                sb.AppendLine("<head>");
                sb.AppendLine("  <meta charset=\"UTF-8\">");
                sb.AppendLine("  <title>Confirmação de Reserva - </title>");
                sb.AppendLine("  <style>");
                sb.AppendLine("    body {");
                sb.AppendLine("      font-family: Arial, sans-serif;");
                sb.AppendLine("      margin: 0;");
                sb.AppendLine("      padding: 0;");
                sb.AppendLine("    }");
                sb.AppendLine("    .container {");
                sb.AppendLine("      width: 600px;");
                sb.AppendLine("      margin: 0 auto;");
                sb.AppendLine("      padding: 20px;");
                sb.AppendLine("      border: 1px solid #ccc;");
                sb.AppendLine("    }");
                sb.AppendLine("    .header {");
                sb.AppendLine("      text-align: center;");
                sb.AppendLine("      margin-bottom: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .logo {");
                sb.AppendLine("      max-width: 200px;");
                sb.AppendLine("      height: auto;");
                sb.AppendLine("    }");
                sb.AppendLine("    .title {");
                sb.AppendLine("      font-size: 24px;");
                sb.AppendLine("      font-weight: bold;");
                sb.AppendLine("    }");
                sb.AppendLine("    .details {");
                sb.AppendLine("      margin-bottom: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .label {");
                sb.AppendLine("      font-weight: bold;");
                sb.AppendLine("    }");
                sb.AppendLine("    .value {");
                sb.AppendLine("      margin-left: 10px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .information {");
                sb.AppendLine("      margin-bottom: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .address {");
                sb.AppendLine("      font-style: italic;");
                sb.AppendLine("    }");
                sb.AppendLine("    .notes {");
                sb.AppendLine("      font-size: 12px;");
                sb.AppendLine("      margin-top: 10px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .footer {");
                sb.AppendLine("      text-align: center;");
                sb.AppendLine("      margin-top: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("  </style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine("  <div class=\"container\">");
                sb.AppendLine("    <div class=\"header\">");
                sb.AppendLine("      <h1 class=\"title\">Confirmação de Reserva</h1>");
                sb.AppendLine("    </div>");

                sb.AppendLine("    <div class=\"details\">");
                sb.AppendLine("      <p class=\"label\">Prezado(a):</p>");
                //sb.AppendLine("      <p class=\"value\">" + guestName + "</p>");

                sb.AppendLine("      <p class=\"label\">Detalhes da Reserva:</p>");
                sb.AppendLine("      <ul>");
                //sb.AppendLine("        <li><span class=\"label\">Hotel:</span> " + hotelName + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Data de Chegada:</span> " + arrivalDate + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Data de Partida:</span> " + departureDate + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Nome do Hóspede:</span> " + guestName + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Número de Quartos:</span> " + numRooms + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Tipo de Quarto:</span> " + roomType + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Valor Total:</span> " + totalCost.ToString("C") + "</li>"); // Format currency
                sb.AppendLine("      </ul>");
                sb.AppendLine("    </div>");

                sb.AppendLine("    <div class=\"information\">");
                sb.AppendLine("      <p class=\"label\">Informações do Hotel:</p>");
                sb.AppendLine("      <ul>");
                //sb.AppendLine("        <li><span class=\"label\">Endereço:</span> " + hotelAddress + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Telefone:</span> " + hotelPhone + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">E-mail:</span> " + hotelEmail + "</li>");
                sb.AppendLine("      </ul>");
                sb.AppendLine("    </div>");

                sb.AppendLine("    <div class=\"notes\">");
                //sb.AppendLine("      <p>" + notes + "</p>");  // Insert your notes here
                sb.AppendLine("    </div>");

                sb.AppendLine("    <div class=\"footer\">");
                sb.AppendLine("      <p>Agradecemos sua escolha e esperamos recebê-lo(a) em breve!</p>");
                sb.AppendLine("      <p>Atenciosamente,</p>");
                //sb.AppendLine("      <p>Equipe do " + hotelName + "</p>");

                sb.AppendLine("      <p>P.S.: Para garantir sua reserva, pedimos que responda a este e-mail confirmando sua chegada.</p>");
                sb.AppendLine("    </div>");
                sb.AppendLine("  </div>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = sb.ToString()
                };
                //MimeMessage is ready, now send the Email.
                using (var client = new SmtpClient())
                {
                    client.Connect(_configuration["EmailSettings:Host"],Int32.Parse(_configuration["EmailSettings:Port"]));
                    client.Authenticate(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    client.Send(email);
                    client.Disconnect(true);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"---> Houve Falha: {ex.Message}");
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> SendEmailCancelamento(EmailSettings emailSetting)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_configuration["EmailSettings:FromName"], _configuration["EmailSettings:FromEmail"]));
                email.To.Add(new MailboxAddress(emailSetting.Email, emailSetting.Email));
                email.Subject = "Cancelametno da Reserva do Quarto Tal";
                email.Priority = MessagePriority.Urgent;

                StringBuilder sb = new();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang=\"pt-br\">");
                sb.AppendLine("<head>");
                sb.AppendLine("  <meta charset=\"UTF-8\">");
                sb.AppendLine("  <title>Cancelamento de Reserva - </title>");
                sb.AppendLine("  <style>");
                sb.AppendLine("    body {");
                sb.AppendLine("      font-family: Arial, sans-serif;");
                sb.AppendLine("      margin: 0;");
                sb.AppendLine("      padding: 0;");
                sb.AppendLine("    }");
                sb.AppendLine("    .container {");
                sb.AppendLine("      width: 600px;");
                sb.AppendLine("      margin: 0 auto;");
                sb.AppendLine("      padding: 20px;");
                sb.AppendLine("      border: 1px solid #ccc;");
                sb.AppendLine("    }");
                sb.AppendLine("    .header {");
                sb.AppendLine("      text-align: center;");
                sb.AppendLine("      margin-bottom: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .logo {");
                sb.AppendLine("      max-width: 200px;");
                sb.AppendLine("      height: auto;");
                sb.AppendLine("    }");
                sb.AppendLine("    .title {");
                sb.AppendLine("      font-size: 24px;");
                sb.AppendLine("      font-weight: bold;");
                sb.AppendLine("    }");
                sb.AppendLine("    .details {");
                sb.AppendLine("      margin-bottom: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .label {");
                sb.AppendLine("      font-weight: bold;");
                sb.AppendLine("    }");
                sb.AppendLine("    .value {");
                sb.AppendLine("      margin-left: 10px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .information {");
                sb.AppendLine("      margin-bottom: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .address {");
                sb.AppendLine("      font-style: italic;");
                sb.AppendLine("    }");
                sb.AppendLine("    .notes {");
                sb.AppendLine("      font-size: 12px;");
                sb.AppendLine("      margin-top: 10px;");
                sb.AppendLine("    }");
                sb.AppendLine("    .footer {");
                sb.AppendLine("      text-align: center;");
                sb.AppendLine("      margin-top: 20px;");
                sb.AppendLine("    }");
                sb.AppendLine("  </style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine("  <div class=\"container\">");
                sb.AppendLine("    <div class=\"header\">");
                sb.AppendLine("      <h1 class=\"title\">Cancelamento de Reserva</h1>");
                sb.AppendLine("    </div>");

                sb.AppendLine("    <div class=\"details\">");
                sb.AppendLine("      <p class=\"label\">Prezado(a):</p>");
                //sb.AppendLine("      <p class=\"value\">" + guestName + "</p>");

                sb.AppendLine("      <p class=\"label\">Detalhes do cancelamento:</p>");
                sb.AppendLine("      <ul>");
                //sb.AppendLine("        <li><span class=\"label\">Hotel:</span> " + hotelName + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Data de Chegada:</span> " + arrivalDate + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Data de Partida:</span> " + departureDate + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Nome do Hóspede:</span> " + guestName + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Número de Quartos:</span> " + numRooms + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Tipo de Quarto:</span> " + roomType + "</li>");
                //sb.AppendLine("        <li><span class=\"label\">Valor Total:</span> " + totalCost.ToString("C") + "</li>"); // Format currency
                sb.AppendLine("      </ul>");
                sb.AppendLine("    </div>");

                sb.AppendLine("    <div class=\"notes\">");
                //sb.AppendLine("      <p>" + notes + "</p>");  // Insert your notes here
                sb.AppendLine("    </div>");

                sb.AppendLine("    <div class=\"footer\">");
                sb.AppendLine("      <p>Lamentamos sua escolha e esperamos recebê-lo(a) novamente em breve!</p>");
                sb.AppendLine("      <p>Atenciosamente,</p>");
                sb.AppendLine("      <p>Equipe do Roubavago</p>");
                sb.AppendLine("    </div>");
                sb.AppendLine("  </div>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = sb.ToString()
                };
                //MimeMessage is ready, now send the Email.
                using (var client = new SmtpClient())
                {
                    client.Connect(_configuration["EmailSettings:Host"],Int32.Parse(_configuration["EmailSettings:Port"]));
                    client.Authenticate(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    client.Send(email);
                    client.Disconnect(true);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"---> Houve Falha: {ex.Message}");
            }

            return Ok();
        }
    }
}