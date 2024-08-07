namespace inventory.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public bool IsOutOfStock { get; set; }
    public string? Secret { get; set; }
}