using System.Linq;
using GTANetworkAPI;
using ServerSide.Database.Handlers;
using ServerSide.Database.Models;
using ServerSide.Enums;
using ServerSide.Extensions;
using ServerSide.Inventory.Items;
using ServerSide.Services.InventoryService;

namespace ServerSide.EventsHandlers.Inventory;

public class EventsUseItem : Script
{
    [RemoteEvent("CEF::SERVER:USE_ITEM")]
    public void OnUseItem(Player player, int idItem)
    {
        var inventory = player.GetInventory();
        var item = inventory.FirstOrDefault(i => i.ItemType.IdItem == idItem);
        if (item.ItemType is Food food)
        {
            food.Use(player);
            var character = player.GetCharacter();
            character.RemoveItem(item, 1);
            player.UpdateInventoryCef();
        }
    }
    [RemoteEvent("CEF::SERVER:DROP_ITEM_PLAYER")]
    public void OnDropItem(Player player, int idItem, int count)
    {
        if (player.IsInVehicle)
        {
            player.SendNotify(NotifyType.Warning, "Вы не можете это сделать находясь в машине!");
            return;
        }
        if(count == 0)return;
        
        var inventory = player.GetInventory();
        var item = inventory.FirstOrDefault(i => i.ItemType.IdItem == idItem);
        if (item != null)
        {
            item.DropItem(player,count);
            player.PlayAnimation("anim@heists@narcotics@trash","pickup_45_r",0);
        }
    }

    [RemoteEvent("CLIENT::SERVER:ON_PICKUP_ITEM")]
    public async void OnPickupItem(Player player)
    {
        if (await player.IsAnimPlaying("pickup_object", "pickup_low", 15)) return;
        if (player.IsInVehicle)return;
        var itemBase = ItemService.GetClosestItemBase(player);
        if (itemBase != null)
        {
            ItemService.DestroyItem(itemBase);
            player.GetCharacter().AddItem(itemBase.ItemType, itemBase.Count);
            player.PlayAnimation("pickup_object", "pickup_low", 15);
            NAPI.Task.Run(() =>
            {
                player.StopAnimation();
            }, 1900);
        }
    }
}