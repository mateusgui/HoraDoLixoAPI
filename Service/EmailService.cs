using HoraDoLixo.ServiceInterface;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HoraDoLixo.Service
{
    /// <summary>
    /// Esta é a implementação final do serviço de e-mail,
    /// utilizando o SendGrid para garantir a entrega.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarEmailAsync(string paraEmail, string assunto, string corpoHtml)
        {
            try
            {
                // Pega a chave de API do SendGrid do appsettings.json
                var apiKey = _configuration["EmailSettings:SendGridApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    Console.WriteLine("ERRO: A chave de API do SendGrid não está configurada no appsettings.json");
                    return;
                }

                var client = new SendGridClient(apiKey);

                var from = new EmailAddress(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderName"]);

                var to = new EmailAddress(paraEmail);

                // Cria a mensagem usando os ajudantes do SendGrid
                var msg = MailHelper.CreateSingleEmail(from, to, assunto, "", corpoHtml);

                // Envia o e-mail
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"E-mail enviado com sucesso para {paraEmail} via SendGrid!");
                }
                else
                {
                    // Mostra o erro caso o SendGrid recuse o envio
                    Console.WriteLine($"Falha ao enviar e-mail via SendGrid: {response.StatusCode}");
                    string responseBody = await response.Body.ReadAsStringAsync();
                    Console.WriteLine($"Detalhes do erro: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção ao tentar enviar e-mail para {paraEmail}: {ex.Message}");
            }
        }
    }
}