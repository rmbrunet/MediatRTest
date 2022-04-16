namespace Models.Customers;


public class Customer
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerNumber { get; set; }
    public string? PriceLevel { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingState { get; set; }
    public string? BillingCountry { get; set; }
    public string? BillingZip { get; set; }
}