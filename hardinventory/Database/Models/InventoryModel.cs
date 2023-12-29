using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using ServerSide.Database.Models.Interfaces;

namespace ServerSide.Database.Models;

public abstract class InventoryModel<T>: IBaseModel where T: InventoryModel<T>
{
    public int Id { get; set; }
    [NotMapped]
    public List<ItemBase> Inventory
    {
        get
        {
            return JsonConvert.DeserializeObject<List<ItemBase>>(InventoryJson);
        }
        set
        {
            InventoryJson = JsonConvert.SerializeObject(value);
        }
    }
    public string InventoryJson { get; set; }
    public abstract int MaxCountItems { get; set; }
    public void AddItem(ItemType itemType, int count = 1)
    {
        var newInventory = Inventory;
        var searchedItem = this.Inventory.FirstOrDefault(i => i.ItemType.IdItem == itemType.IdItem);
        var index = this.Inventory.FindIndex(i=>i.ItemType.IdItem == itemType.IdItem);
        if (searchedItem!=null)
        {
            searchedItem.Count += count;
            newInventory[index] = searchedItem;
            Inventory = newInventory;
            Update();
            return;
        }
        newInventory.Add(new ItemBase(count, itemType));
        Inventory = newInventory;
        Update();
    }
    public void RemoveItem(ItemBase item, int count)
    {
        var newInventory = Inventory;
        var searchedItem = this.Inventory.FirstOrDefault(i => i.ItemType.IdItem == item.ItemType.IdItem);
        var index = this.Inventory.FindIndex(i=>i.ItemType.IdItem == item.ItemType.IdItem);
        if(searchedItem==null)return;
        if ((searchedItem.Count - count) <= 0)
        {
            newInventory.RemoveAt(index);
            Inventory = newInventory;
            Update();
            return;
        }
        searchedItem.Count -= count;
        newInventory[index] = searchedItem;
        Inventory = newInventory;
        Update();
    }
    private void Update()
    {
        using Context db = new Context();
        var entity = db.Find<T>(Id);
        entity.Inventory = Inventory;
        db.Update(entity);
        db.SaveChanges();
    }
}