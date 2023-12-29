using System.Collections.Generic;
using System.Linq;
using ServerSide.Database.Models;

namespace ServerSide.Database.Handlers;

public class ItemTypeHandler
{
    public static ItemType GetItemByIdItem(int idItem)
    {
        using Context db = new Context();
        return db.ItemsTypes.FirstOrDefault(i => i.IdItem == idItem);
    }

    public static void AddNewItemType(ItemType itemType)
    {
        using Context db = new Context();
        db.ItemsTypes.Add(itemType);
        db.SaveChanges();
    }

    public static List<ItemType> GetItemTypes()
    {
        using Context db = new Context();
        return db.ItemsTypes.ToList();
    }
}