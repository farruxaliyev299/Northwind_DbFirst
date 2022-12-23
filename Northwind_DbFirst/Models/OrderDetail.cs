namespace DB_Experiments.Models;

public class OrderDetail
{

   public int OrderID { get; set; }

   public int ProductID { get; set; }

   public decimal UnitPrice { get; set; }

   public short Quantity { get; set; }

   public double Discount { get; set; }

}
