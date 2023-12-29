using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using ServerSide.Database.Models;

namespace ServerSide.Services.InventoryService;

public class ItemService : Script
{
    public static readonly List<DroppedItemModel> DroppedItems = new List<DroppedItemModel>()
    {
        
    };
    public static void SpawnItem(ItemBase itemBase, Vector3 position, uint dimension, int count)
    {
        var o = NAPI.Object.CreateObject(itemBase.ItemType.Hash, new Vector3(position.X, position.Y, position.Z - 1.01f), new Vector3(), dimension: dimension);
        var textLabel = NAPI.TextLabel.CreateTextLabel($"{itemBase.ItemType.Name} {count} шт.",
            new Vector3(position.X, position.Y, position.Z - 0.5f), 10.0f, 0.45f, 4,
            new Color(255, 255, 255));
        DroppedItems.Add(new DroppedItemModel(itemBase, o, textLabel, dimension));
    }
    public static void DestroyItem(ItemBase itemBase)
    {
        var droppedItem = DroppedItems.FirstOrDefault(i => i.ItemBase == itemBase);
        droppedItem.TextLabel.Delete();
        droppedItem.Object.Delete();
        DroppedItems.Remove(droppedItem);
    }

    public static ItemBase GetClosestItemBase(Player player)
    {
        ItemBase itemBase = null;
        foreach (var droppedItem in DroppedItems)
        {
            if (player.Position.DistanceTo(droppedItem.Object.Position) < 1.8f)
            {
                itemBase = droppedItem.ItemBase;
            }
        }
        return itemBase;
    }
}