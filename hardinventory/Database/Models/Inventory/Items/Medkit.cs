using GTANetworkAPI;
using ServerSide.Database.Models;

namespace ServerSide.Inventory.Items;

public class Medkit : ItemType
{
    public int CountHp { get; set; }
    public Medkit(string name, string description, int hash, int idItem,int countHp): base(name, description, hash, idItem)
    {
        CountHp = countHp;
    }

    public Medkit()
    {
        
    }
    public void Use(Player player)
    {
        //хилимся
        player.Health += CountHp;
    }
}