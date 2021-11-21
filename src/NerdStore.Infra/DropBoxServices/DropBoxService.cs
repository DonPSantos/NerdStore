using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NerdStore.Infra.DropBoxServices.Interfaces;

namespace NerdStore.Infra.DropBoxServices
{
    public class DropBoxService : IDropBoxService
    {
        public IConfiguration _configuration { get; }

        public DropBoxService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadArquivo(IFormFile arquivo)
        {
            var accessToken = _configuration.GetSection("DropBoxToken").Value;

            using (var _dropBox = new DropboxClient(accessToken))
            using (var _memoryStream = new MemoryStream())
            {
                await arquivo.CopyToAsync(_memoryStream);
                _memoryStream.Position = 0;
                var updated = await _dropBox.Files.UploadAsync("/" + arquivo.FileName, WriteMode.Overwrite.Instance, body: _memoryStream);
                var result = await _dropBox.Sharing.CreateSharedLinkWithSettingsAsync("/" + arquivo.FileName);
                return result.Url + "&raw=1";
            }
        }

        public async Task ApagarImagem(string urlImagem)
        {
            var accessToken = _configuration.GetSection("DropBoxToken").Value;

            using (var _dropBox = new DropboxClient(accessToken))
                await _dropBox.Files.DeleteV2Async("/" + urlImagem);
        }
    }
}
