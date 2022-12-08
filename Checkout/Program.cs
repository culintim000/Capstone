using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Steeltoe.Discovery.Client;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

var client = new MongoClient(builder.Configuration.GetConnectionString("Database1"));
var _db = client.GetDatabase("ECommerce");
var collection = _db.GetCollection<Order>("Orders");

app.MapGet("/", () => "Hello World!");
app.MapPost("/", (string cartid) => {
    var carts = _db.GetCollection<UserCart>("Carts");
    UserCart cart = carts.Find(cart => cart._id == ObjectId.Parse(cartid)).ToList().FirstOrDefault();
    if(cart == null) return Results.BadRequest("invalid cart id");
    Order order = new();
    order.Cart = cart;
    collection.InsertOne(order);

    carts.DeleteOneAsync(cart => cart._id == ObjectId.Parse(cartid));
    return Results.Ok("Order Placed");
});

app.Run();
