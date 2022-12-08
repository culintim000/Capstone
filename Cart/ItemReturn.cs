
public class ItemReturn {
    public string _id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }

    public ItemReturn(Item item)
    {
        this._id = item._id.ToString();
        this.Name = item.Name;
        this.Description = item.Description;
        this.Price = item.Price;
    }
}