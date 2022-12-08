using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Controllers
{
    [ApiController]
    [Route("")]
    public class InventoryController : ControllerBase
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<Item> _col;
        public InventoryController(IConfiguration config) {
            var client = new MongoClient(config.GetConnectionString("Database1"));
            _db = client.GetDatabase("ECommerce");
            _col = _db.GetCollection<Item>("Inventory");
        }

        [HttpPost]
        public async Task<IResult> AddItem (Item item)
        {
            await _col.InsertOneAsync(item);

            return Results.Ok(item._id.ToString());
        }

        [HttpGet]
        public async Task<IResult> GetAllItems()
        {
            var allItems = await _col.Find(_ => true).ToListAsync();

            if (allItems == null){
                return Results.Ok("No Items Found"); 
            }

            var itemsToReturn = allItems.Select(item => new ItemToReturn(item));

            return Results.Ok(itemsToReturn);
        }

        [HttpGet]
        [Route("search/{namePassedIn}")]
        public async Task<IResult> SearchItemsWithName(string namePassedIn)
        {
            var entity = await _col.Find(document => document.Name.ToLower().Contains(namePassedIn.ToLower())).ToListAsync();
            
            if(entity == null){
                return Results.Ok("No Items Found");
            }

            var itemsToReturn = entity.Select(item => new ItemToReturn(item));
            return Results.Ok(itemsToReturn);
        }

        [HttpGet]
        [Route("{idPassedIn}")]
        public async Task<IResult> SearchItemsWithID(string idPassedIn)
        {
            var entity = _col.Find(document => document._id == ObjectId.Parse(idPassedIn)).FirstOrDefault();
            
            if(entity == null){
                return Results.Ok("No Items Found");
            }
            return Results.Ok(new ItemToReturn(entity));
        }

        [HttpPut]
        [Route("{idPassedIn}")]
        public async Task<ActionResult<UpdateResult>> UpdateItemsWithID(string idPassedIn, Item itemPassedIn)
        {
            var filter = Builders<Item>.Filter.Eq(s => s._id, ObjectId.Parse(idPassedIn));

            var update = Builders<Item>.Update.Set(s => s.Description, itemPassedIn.Description);
            var update1 = Builders<Item>.Update.Set(s => s.Name, itemPassedIn.Name);
            var update2 = Builders<Item>.Update.Set(s => s.Price, itemPassedIn.Price);

            var result = await _col.UpdateOneAsync(filter, update);
            var result1 = await _col.UpdateOneAsync(filter, update1);
            var result2 = await _col.UpdateOneAsync(filter, update2);

            return result;
        }

        
        [HttpDelete]
        [Route("{idPassedIn}")]
        public async Task<IResult> DeleteItemsWithID(string idPassedIn)
        {
            var filter = Builders<Item>.Filter.Eq(s => s._id, ObjectId.Parse(idPassedIn));
            var result = _col.DeleteOneAsync(filter);
            
            return Results.Ok("Item Deleted");
        }

    }
}