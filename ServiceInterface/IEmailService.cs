namespace HoraDoLixo.ServiceInterface
{
    /// <summary>
    /// Define o contrato para um serviço que envia e-mails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envia um e-mail de forma assíncrona.
        /// </summary>
        /// <param name="paraEmail">O e-mail do destinatário.</param>
        /// <param name="assunto">O assunto do e-mail.</param>
        /// <param name="corpoHtml">O corpo do e-mail, que pode conter HTML.</param>
        Task EnviarEmailAsync(string paraEmail, string assunto, string corpoHtml);
    }
}
