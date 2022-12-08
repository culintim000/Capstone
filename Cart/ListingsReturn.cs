
public class ListingReturn {
    public ItemReturn? ItemReturn { get; set; }
    public int Quantity { get; set; } = 0;
    
    public ListingReturn(Listing listing) {
        this.Quantity = listing.Quantity;
        this.ItemReturn = new ItemReturn(listing.Item);
    }
}