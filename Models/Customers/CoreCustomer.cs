namespace Models.Customers;

public class CoreCustomer
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerNumber { get; set; }
    public string? PriceLevel { get; set; }
    public CoreAddress? BillingAddress { get; set; }
}