using GTANetworkAPI;
using ServerSide.Database.Models;

namespace ServerSide.Services.InventoryService;

public class DroppedItemModel
{
    public ItemBase ItemBase { get; set; }
    public Object Object { get; set; }
    public TextLabel TextLabel { get; set; }
    public uint Dimension { get; set; }

    public DroppedItemModel(ItemBase itemBase, Object o, TextLabel textLabel, uint dimension)
    {
        ItemBase = itemBase;
        Object = o;
        TextLabel = textLabel;
        Dimension = dimension;
    }
}