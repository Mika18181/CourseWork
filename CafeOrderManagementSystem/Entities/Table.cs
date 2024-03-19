namespace CafeOrderManagementSystem.Entities;

public class Table
{
    public int TableId { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }

    public override string ToString()
    {
        return Number.ToString();
    }
}