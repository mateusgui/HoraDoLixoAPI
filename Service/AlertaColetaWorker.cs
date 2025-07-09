using HoraDoLixo.ServiceInterface;

namespace HoraDoLixo.Service
{
    /// <summary>
    /// Este é um serviço que roda em segundo plano (background) e de forma contínua.
    /// Sua única responsabilidade é verificar periodicamente se é hora de enviar
    /// algum alerta de coleta de lixo para os usuários.
    /// </summary>
    public class AlertaColetaWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AlertaColetaWorker> _logger;

        public AlertaColetaWorker(IServiceProvider serviceProvider, ILogger<AlertaColetaWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de Alerta de Coleta está iniciando.");

            // Este loop infinito garante que o serviço rode enquanto a aplicação estiver ativa.
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Usamos um 'scope' para garantir que os serviços (que são Scoped)
                    // sejam criados e descartados corretamente a cada ciclo.
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var usuarioService = scope.ServiceProvider.GetRequiredService<IUsuarioService>();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                        // Chamamos o método que contém toda a lógica de verificação e envio.
                        await ProcessarAlertas(usuarioService, emailService);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocorreu um erro no ciclo do Worker de Alerta de Coleta.");
                }

                // Espera 1 minuto para verificar novamente.
                // Não sobrecarrega o sistema.
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessarAlertas(IUsuarioService usuarioService, IEmailService emailService)
        {
            var horaAtual = DateTime.Now.TimeOfDay;
            // O .NET DayOfWeek começa com Domingo = 0. O banco de dados geralmente usa Domingo = 1.
            // Ajustamos para ficar compatível.
            var diaDaSemanaHoje = (int)DateTime.Now.DayOfWeek + 1;

            _logger.LogInformation("Verificando alertas para o dia {Dia} às {Hora}", diaDaSemanaHoje, horaAtual.ToString("hh\\:mm"));

            var usuariosComAlertaAtivo = usuarioService.GetAll()
                .Where(u => u.Status && (u.AlertaComumAtivo || u.AlertaSeletivaAtivo));

            foreach (var usuario in usuariosComAlertaAtivo)
            {
                var infoColeta = await usuarioService.GetInformacoesColetaAsync(usuario.IdUsuario);
                if (infoColeta == null) continue;

                // --- LÓGICA PARA COLETA COMUM ---
                if (usuario.AlertaComumAtivo && infoColeta.ColetaComum != null && usuario.HorarioAlertaComum.HasValue)
                {
                    var programacaoHoje = infoColeta.ColetaComum.Programacao
                        .FirstOrDefault(p => p.DiaSemana == diaDaSemanaHoje);

                    // Verifica se há coleta hoje e se bateu o horário do alerta do usuário
                    if (programacaoHoje != null &&
                        usuario.HorarioAlertaComum.Value.Hours == horaAtual.Hours &&
                        usuario.HorarioAlertaComum.Value.Minutes == horaAtual.Minutes)
                    {
                        _logger.LogInformation($"Disparando alerta de coleta COMUM para o usuário: {usuario.Email}");
                        string assunto = $"Lembrete: Coleta de Lixo Comum na sua região!";
                        string corpo = $"Olá, {usuario.NomeCompleto}!<br><br>A coleta de lixo comum na sua região ({infoColeta.ColetaComum.Nome}) está programada para hoje, com início previsto para as {programacaoHoje.HorarioInicioPrevisto:hh\\:mm}.<br><br>Não se esqueça de colocar o lixo para fora com antecedência.<br><br>Atenciosamente,<br>Equipe Hora do Lixo";

                        await emailService.EnviarEmailAsync(usuario.Email, assunto, corpo);
                    }
                }

                // --- LÓGICA PARA COLETA SELETIVA ---
                if (usuario.AlertaSeletivaAtivo && infoColeta.ColetaSeletiva != null && usuario.HorarioAlertaSeletiva.HasValue)
                {
                    var programacaoHoje = infoColeta.ColetaSeletiva.Programacao
                        .FirstOrDefault(p => p.DiaSemana == diaDaSemanaHoje);

                    if (programacaoHoje != null &&
                        usuario.HorarioAlertaSeletiva.Value.Hours == horaAtual.Hours &&
                        usuario.HorarioAlertaSeletiva.Value.Minutes == horaAtual.Minutes)
                    {
                        _logger.LogInformation($"Disparando alerta de coleta SELETIVA para o usuário: {usuario.Email}");
                        string assunto = $"Atenção: Hoje é dia de Coleta Seletiva!";
                        string corpo = $"Olá, {usuario.NomeCompleto}!<br><br>Lembrete de que a coleta de lixo seletivo na sua região ({infoColeta.ColetaSeletiva.Nome}) está programada para hoje, com início previsto para as {programacaoHoje.HorarioInicioPrevisto:hh\\:mm}.<br><br>Separe seus recicláveis!<br><br>Atenciosamente,<br>Equipe Hora do Lixo";

                        await emailService.EnviarEmailAsync(usuario.Email, assunto, corpo);
                    }
                }
            }
        }
    }
}