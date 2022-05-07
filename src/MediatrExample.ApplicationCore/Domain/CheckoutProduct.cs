namespace MediatrExample.ApplicationCore.Domain;
public class CheckoutProduct : BaseEntity
{
    public int CheckoutProductId { get; set; }
    public int CheckoutId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double Total { get; set; }
    public Checkout Checkout { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
