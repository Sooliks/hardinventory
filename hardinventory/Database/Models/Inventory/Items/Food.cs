using GTANetworkAPI;
using ServerSide.Database.Handlers;
using ServerSide.Database.Models;
using ServerSide.Enums;
using ServerSide.Extensions;

namespace ServerSide.Inventory.Items;

public class Food : ItemType
{
    public byte CountSatiety { get; set; }
    public Food(string name, string description, int hash, int idItem, byte countSatiety): base(name, description, hash, idItem)
    {
        CountSatiety = countSatiety;
    }

    public Food()
    {
        
    }
    public void Use(Player player)
    {
        var character = player.GetCharacter();
        if (character.CountSatiety + CountSatiety >= 100)
        {
            CharacterHandler.SetSatiety(character,100);
            player.SendNotify(NotifyType.Info, "Cытость восстановлена на 100%");
            return;
        }
        CharacterHandler.SetSatiety(character,character.CountSatiety+=CountSatiety);
        player.SendNotify(NotifyType.Info, $"Cытость восстановлена на {character.CountSatiety+CountSatiety}%");
    }
}