using System.Globalization;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Export;

public class ExportSaleDto
{
    private decimal price;

    private decimal discount;
    
    [JsonProperty("car")]
    public ExportCarDto Car { get; set; } = null!;

    [JsonProperty("customerName")]
    public string CustomerName { get; set; } = null!;

    [JsonProperty("discount")] 
    public string Discount
    {
        get { return this.discount.ToString("F2");}
        set { this.discount = decimal.Parse(value, CultureInfo.InvariantCulture); } 
    }

    [JsonProperty("price")]
    public string Price
    {
        get { return this.price.ToString("F2");} 
        set { this.price = Decimal.Parse(value, CultureInfo.InvariantCulture);} }

    [JsonProperty("priceWithDiscount")]
    public string PriceWithDiscount
        => this.CalculatePriceWithDiscount(this.price, this.discount);

    private string CalculatePriceWithDiscount(decimal price, decimal discount)
    {

        decimal priceWithDiscount = price - (price * (discount / 100));

        return priceWithDiscount.ToString("F2");
    }
}