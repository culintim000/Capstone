public class ItemToReturn
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }

    public ItemToReturn(Item item)
    {
        Id = item._id.ToString();
        Name = item.Name;
        Description = item.Description;
        Price = item.Price;
    }
}