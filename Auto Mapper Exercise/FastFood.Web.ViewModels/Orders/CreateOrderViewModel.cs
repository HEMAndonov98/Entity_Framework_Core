namespace FastFood.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class CreateOrderViewModel
    {

        public CreateOrderViewModel()
        {
            this.EmployeesNames = new Dictionary<int, string>();
            this.ItemsNames = new Dictionary<int, string>();
        }
        public IDictionary<int, string> ItemsNames { get; set; } = null!;

        public IDictionary<int, string> EmployeesNames { get; set; } = null!;

    }
}
