using MongoDB.Bson;

public class UserCartReturning {
    public string _id { get; set; }
    public List<ListingReturn> Lines { get; set; } = new();
    public UserCartReturning(UserCart cart) {
        this._id = cart._id.ToString();
        foreach (Listing line in cart.Lines) {
            this.Lines.Add(new ListingReturn(line));
        }
    }
}