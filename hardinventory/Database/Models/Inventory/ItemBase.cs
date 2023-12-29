using System;
using GTANetworkAPI;
using ServerSide.Database.Handlers;
using ServerSide.Extensions;
using ServerSide.Services.InventoryService;

namespace ServerSide.Database.Models;

public class ItemBase
{
    public int Id { get; set; }
    public int Count { get; set; }
    public ItemType ItemType { get; set; }
    public int IndexBoard { get; set; }
    public ItemBase(int count, ItemType item)
    {
        Count = count;
        ItemType = item;
    }

    public ItemBase()
    {
        
    }
    
    public virtual void DropItem(Player player, int count)
    {
        if (count > Count || count < 1)
        {
            return;
        }

        var character = player.GetCharacter();
        character.RemoveItem(this, count);
        var newItemBase = this;
        newItemBase.Count = count;
        ItemService.SpawnItem(newItemBase, player.Position, player.Dimension, count);
        player.UpdateInventoryCef();
    }
}