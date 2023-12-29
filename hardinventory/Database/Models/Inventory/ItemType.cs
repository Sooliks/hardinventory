namespace ServerSide.Database.Models;

public class ItemType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Hash { get; set; }
    public int IdItem { get; set; }

    public ItemType()
    {
        
    }
    public ItemType(string name, string description, int hash, int idItem)
    {
        Name = name;
        Description = description;
        Hash = hash;
        IdItem = idItem;
    }
}