namespace MediatRTest.Model;

public class Customer
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerNumber { get; set; }
    public string? PriceLevel { get; set; }
    public Address? BillingAddress { get; set; }
}