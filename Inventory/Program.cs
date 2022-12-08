using Steeltoe.Discovery.Client;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDiscoveryClient(builder.Configuration); //for eureka client
builder.Services.AddCors();
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

app.MapGet("/test", () => {
    return "Hello from test";
});

// app.MapGet("/testEureka", async (IDiscoveryClient idc) =>
//     {
//         //return "this is the root of dotnet-eureka-client";
//         DiscoveryHttpClientHandler _handler = new DiscoveryHttpClientHandler(idc);
//         var client = new HttpClient(_handler, false);
//         return await client.GetStringAsync("http://CART-API/") + " <-- that is from the Cart API. This is from the Inventory API";
//     }
// );

app.Run();

