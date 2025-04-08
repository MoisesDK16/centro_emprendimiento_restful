using Application.DTOs.Correos;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Application.Services.ExternalS
{
    public class CorreoServicio
    {
        private static string _Host = "smtp.gmail.com";
        private static int _Puerto = 587;

        private static string _NombreEnvia = "Ciemi Uniandes";
        private static string _Correo = "moisesloor122@gmail.com";
        private static string _Clave = "yrdv pxtz xqqh skze";

        public static bool Enviar(CorreoDTO correodto)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_NombreEnvia, _Correo));
                email.To.Add(MailboxAddress.Parse(correodto.Para));
                email.Subject = correodto.Asunto;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = correodto.Contenido
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_Host, _Puerto, SecureSocketOptions.StartTls);

                    smtp.Authenticate(_Correo, _Clave);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return true;
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine($"Error SMTP Command: {ex.Message}");
                Console.WriteLine($"StatusCode: {ex.StatusCode}");
                return false;
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"Error de protocolo SMTP: {ex.Message}");
                return false;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"Error de autenticación SMTP: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al enviar el correo: {ex.Message}");
                return false;
            }
        }


        public static string GenerarToken()
        {
            string token = Guid.NewGuid().ToString("N");
            return token;
        }
    }
}
