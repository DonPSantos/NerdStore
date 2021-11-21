using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class ByteToIFormFile : ITypeConverter<byte[], IFormFile>
    {
        public IFormFile Convert(byte[] source, IFormFile destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
