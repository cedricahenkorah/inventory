namespace inventory.Models;

public class InventoryItemDTO
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public bool IsOutOfStock { get; set; }

}