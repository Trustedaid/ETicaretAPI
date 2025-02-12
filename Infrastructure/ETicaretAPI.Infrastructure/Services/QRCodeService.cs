using ETicaretAPI.Application.Abstractions.Services;
using QRCoder;

namespace ETicaretAPI.Infrastructure.Services;

public class QRCodeService : IQRCodeService
{
    public byte[] GenerateQRCode(string text)
    {
        var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(data);
        var byteGraphic = qrCode.GetGraphic(10, new byte[] { 0, 0, 0 }, new byte[] { 240, 240, 240 });
        return byteGraphic;
    }
}