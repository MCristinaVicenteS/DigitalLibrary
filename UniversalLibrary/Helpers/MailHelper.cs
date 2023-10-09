using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using MailKit.Net.Smtp;

namespace UniversalLibrary.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Response SendEmail(string to, string subject, string body)
        {
            var nameFrom = _configuration["Mail:NameFrom"];
            var from = _configuration["Mail:From"];
            var smtp = _configuration["Mail:Smtp"];
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];

            if (string.IsNullOrEmpty(nameFrom) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(smtp) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(password))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Incomplete email configuration. Please check the settings."
                };
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            var bodybuilder = new BodyBuilder
            {
                HtmlBody = body,
            };
            message.Body = bodybuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                // Registre a exceção ou retorne uma mensagem de erro apropriada para o usuário.
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Error To Send the Email: {ex.Message}"
                };
            }
        }

        //METODO PARA ENVIAR MENSAGEM AO ADMIN
        public Response SendEmailToAdmin(string to, string subject, string body) 
        {
            var nameFrom = _configuration["Mail:NameFrom"];
            var from = _configuration["Mail:From"];
            var smtp = _configuration["Mail:Smtp"];
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];

            if (string.IsNullOrEmpty(nameFrom) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(smtp) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(password))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Incomplete email configuration. Please check the settings."
                };
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            var bodybuilder = new BodyBuilder
            {
                HtmlBody = body,
            };
            message.Body = bodybuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                // Registre a exceção ou retorne uma mensagem de erro apropriada para o usuário.
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Error To Send the Email: {ex.Message}"
                };
            }
        }



    }
}
