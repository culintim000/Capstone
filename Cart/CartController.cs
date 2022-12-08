using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Steeltoe.Discovery.Client;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery;

namespace Controllers
{
    [ApiController]
    [Route("")]
    public class CartController : ControllerBase
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<UserCart> collection;
        public CartController(IConfiguration config) {
            var client = new MongoClient(config.GetConnectionString("Database1"));
            _db = client.GetDatabase("ECommerce");
            collection = _db.GetCollection<UserCart>("Carts");
        }

        // [HttpGet]
        // [Route("test")]
        // public async Task<string> talkingWithEureka(IDiscoveryClient idc){
        //     //return "this is the root of dotnet-eureka-client";
        //     DiscoveryHttpClientHandler _handler = new DiscoveryHttpClientHandler(idc);
        //     var client = new HttpClient(_handler, false);
        //     return await client.GetStringAsync("http://INVENTORY-API/test") + " <-- that came from inventory api.";
        // }

        [HttpPost]
        public async Task<IResult> AddToCart(string itemid, string userid, int amount = 1) {
            if (string.IsNullOrEmpty(userid)) return Results.BadRequest("No userid");
            if (string.IsNullOrEmpty(itemid)) return Results.BadRequest("No itemid");

            UserCart cart = collection.Find(cart => cart._id == ObjectId.Parse(userid)).ToList().FirstOrDefault();
            if(cart == null) cart = new(userid);

            var inventory = _db.GetCollection<Item>("Inventory");
            Item item = inventory.Find(item => item._id == ObjectId.Parse(itemid)).ToList().FirstOrDefault();

            
            if(cart.Lines.Count() > 0) {
                if(cart.Lines.FindAll(line => line.Item._id == item._id).Any()) {
                    int index = cart.Lines.FindIndex(line => line.Item._id == item._id);
                    cart.Lines[index].Quantity += amount;
                }
                else cart.Lines.Add(new(item, amount));

                await collection.ReplaceOneAsync(cart => cart._id == ObjectId.Parse(userid), cart);
                return Results.Ok(amount + " * " + item.Name + "(s) added.");
            }
            else {
                cart.Lines.Add(new Listing(item, amount));
                await collection.InsertOneAsync(cart);
                return Results.Ok("Cart Generated.");
            }
        }
        [HttpGet] 
        [Route("all")]
        public IResult ShowAll(string userid) {
            if (string.IsNullOrEmpty(userid)) return Results.BadRequest("No userid");

            System.Console.WriteLine("userid: " + userid);
            try{
                UserCart cart = collection.Find(cart => cart._id == ObjectId.Parse(userid)).ToList().FirstOrDefault();
                if(cart == null) cart = new(userid);

                // return Results.Ok(cart);
                return Results.Ok(new UserCartReturning(cart));
            } catch(Exception e) {
                return Results.BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("total")]
        public IResult Total(string userid, string item) {
            if (string.IsNullOrEmpty(userid)) return Results.BadRequest("No userid");

            int index = 0;
            UserCart cart = collection.Find(cart => cart._id == ObjectId.Parse(userid)).ToList().FirstOrDefault();
            if(cart == null) return Results.BadRequest("No cart for user");

            if(int.TryParse(item, out index)) {
                if(index < 0 || index >= cart.Lines.Count()) return Results.BadRequest("item must be an index in bounds");

                return Results.Ok(cart.Lines[index].Item.Price * cart.Lines[index].Quantity);
            }
            if(item.ToLower().Equals("all")) {
                double total = 0;
                foreach(Listing line in cart.Lines) {
                    total += line.Item.Price * line.Quantity;
                }
                return Results.Ok(total);
            }
            return Results.BadRequest("item needs to be an index or 'all'");
        }
        [HttpDelete]
        [Route("remove")]
        public async Task<IResult> Remove(string userid, string item) {
            if (string.IsNullOrEmpty(userid)) return Results.BadRequest("No userid");

            int index = 0;
            UserCart cart = collection.Find(cart => cart._id == ObjectId.Parse(userid)).ToList().FirstOrDefault();
            if (cart == null) return Results.BadRequest("No cart for user: " + userid);

            if (int.TryParse(item, out index))
            {
                if (index < 0 || index >= cart.Lines.Count()) return Results.BadRequest("item must be an index in bounds");

                cart.Lines.RemoveAt(index);
                await collection.ReplaceOneAsync(cart => cart._id == ObjectId.Parse(userid), cart);

                if (cart.Lines.Count() == 0) await collection.DeleteOneAsync(cart => cart._id == ObjectId.Parse(userid));
                return Results.Ok("Removed listing at: " + index);
            }
            if (item.ToLower().Equals("all"))
            {
                cart.Lines.Clear();
                await collection.ReplaceOneAsync(cart => cart._id == ObjectId.Parse(userid), cart);

                return Results.Ok("Removed all items in cart");
            }
            return Results.BadRequest("item needs to be an index or 'all'");
        }
    }
}