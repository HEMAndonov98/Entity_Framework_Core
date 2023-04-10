namespace CarDealer.DTOs.Export;

public class GetTotalSalesByCustomer
{
    public bool IsYoungDriver { get; set; }
    public string FullName { get; set; } = null!;

    public int BoughtCars { get; set; }

    public decimal SpentMoney { get; set; }
}