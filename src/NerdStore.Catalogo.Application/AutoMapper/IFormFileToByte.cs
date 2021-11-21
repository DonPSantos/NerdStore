using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class IFormFileToByte : ITypeConverter<IFormFile, byte[]>
    {
        public byte[] Convert(IFormFile source, byte[] destination, ResolutionContext context)
        {
            var fileBase = (IFormFile)context;

            MemoryStream target = new MemoryStream();
            fileBase.CopyTo(target);
            return target.ToArray();
        }
    }
}
