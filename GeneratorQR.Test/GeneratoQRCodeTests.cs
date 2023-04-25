using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using QRCoder;
using System.Net;
using System.Reflection.Emit;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace GeneratorQR.Test
{
    public class GeneratoQRCodeTests
    {
        [Fact]
        public void TestGenerateQRCode_ReturnsBase64String()
        {
            // Arrange
            string testText = "Hello, world!";
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(testText, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode bitmapByteCode = new BitmapByteQRCode(qrCodeData);
            var bitmap = bitmapByteCode.GetGraphic(25);
            byte[] biteImage;

            using (var ms = new MemoryStream())
            {
                ms.Write(bitmap);
                biteImage = ms.ToArray();
            }

            string expectedBase64String = Convert.ToBase64String(biteImage);

            // Act
            var controller = new QRCodeFakeController();
            string actualBase64String = controller.GenerateQRCode(testText);

            // Assert
            Assert.Equal(expectedBase64String, actualBase64String);
        }
    }

    public class QRCodeFakeController
    {
        public string GenerateQRCode(string text)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeDate = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode bitmapByteCode = new BitmapByteQRCode(qrCodeDate);
            var bitmap = bitmapByteCode.GetGraphic(25);

            using var ms = new MemoryStream();
            ms.Write(bitmap);
            byte[] biteImage = ms.ToArray();

            return Convert.ToBase64String(biteImage);
        }
    }
}

