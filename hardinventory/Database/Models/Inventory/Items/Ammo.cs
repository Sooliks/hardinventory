using ServerSide.Database.Models;
using ServerSide.Inventory.Enums;

namespace ServerSide.Inventory.Items;

public class Ammo : ItemType
{
    public TypeAmmo TypeAmmo { get; set; }
    public Ammo(string name, string description, int hash, int idItem, TypeAmmo typeAmmo): base(name, description, hash, idItem)
    {
        TypeAmmo = typeAmmo;
    }

    public Ammo()
    {
        
    }
}