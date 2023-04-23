using QRCoder;
using System.Net.Mime;

string policy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policy, build =>
    {
        build.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
        .AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(policy);

app.MapGet("/", async (HttpContext context) =>
{
    // Lee el contenido del archivo HTML
    string htmlContent = await File.ReadAllTextAsync("index.html");

    // Establece la respuesta HTTP con el contenido del archivo HTML
    context.Response.Headers.Add("Content-Type", new[] { MediaTypeNames.Text.Html });
    await context.Response.WriteAsync(htmlContent);
});

app.MapGet("/qr", (string text) => 
{
    var qrGenerator = new QRCodeGenerator();
    var qrCodeDate = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
    BitmapByteQRCode bitmapByteCode = new BitmapByteQRCode(qrCodeDate);
    var bitmap = bitmapByteCode.GetGraphic(25);

    using var ms = new MemoryStream();
    ms.Write(bitmap);
    byte[] biteImage = ms.ToArray();

    return Convert.ToBase64String(biteImage);
});

app.Run();