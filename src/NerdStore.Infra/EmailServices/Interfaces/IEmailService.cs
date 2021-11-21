namespace NerdStore.Infra.EmailServices.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmail(string emailDestino, string assunto, string corpo);
    }
}
