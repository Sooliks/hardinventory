
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using ServerSide.Extensions;

namespace ServerSide.EventsHandlers.Inventory;

public class EventsInventory : Script
{
    [RemoteProc("RPC::CEF:SERVER:GetInventoryPlayer")]
    public string GetInventory(Player player)
    {
        var inventory = player.GetInventory().Select(i => new
        {
            count = i.Count, name = i.ItemType.Name, description = i.ItemType.Description, idItem = i.ItemType.IdItem, hash = i.ItemType.Hash, type = i.GetType().Name
        }).ToList();
        return JsonConvert.SerializeObject(inventory);
    }
}