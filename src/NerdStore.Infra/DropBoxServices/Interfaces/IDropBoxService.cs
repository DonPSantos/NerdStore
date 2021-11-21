using Microsoft.AspNetCore.Http;

namespace NerdStore.Infra.DropBoxServices.Interfaces
{
    public interface IDropBoxService
    {
        Task<string> UploadArquivo(IFormFile arquivo);
        Task ApagarImagem(string urlImagem);
    }
}
