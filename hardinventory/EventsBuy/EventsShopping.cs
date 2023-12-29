
using System;
using System.Linq;
using GTANetworkAPI;
using ServerSide.Database.Handlers;
using ServerSide.Database.Handlers.BusinessesHandlers;
using ServerSide.Database.Models;
using ServerSide.Enums;
using ServerSide.Extensions;
using ServerSide.Extensions.VehicleExtensions;
using ServerSide.Services;
using ServerSide.Services.MapService;
using ServerSide.Services.VehicleServices;


namespace ServerSide.EventsHandlers.EventsBuy;

public class EventsShopping : Script
{
    [RemoteEvent("CEF::SERVER:ON_BUY_ITEM")]
    public void OnBuyItem(Player player,int businessId, int idItem)
    {
        if(!player.IsAuthorized())return;
        var business = BusinessHandler.GetBusinessById(businessId);
        if (business != null && business.BusinessType == BusinessesTypes.Market)
        {
            var item = business.Items.FirstOrDefault(i => i.ItemId == idItem);
            if (item.Count < 1)
            {
                player.SendNotify(NotifyType.Warning, "Данный товар закончился на складе");
                return;
            }
            if (player.MinusMoney(item.Price))
            {
                player.GetCharacter().AddItem(ItemTypeHandler.GetItemByIdItem(idItem));
                if (business.OwnerCharacterId != 0)
                {
                    BusinessHandler.RemoveItem(item, business);
                    BusinessHandler.AddMoneyInBank(item.Price, business.Id);
                }
                StatisticBusinessHandler.AddBuyProduct(business);
            }
        }
    }

    [RemoteEvent("CEF::SERVER:BUY_FUEL")]
    public void OnBuyFuel(Player player, int businessId, int itemId, int count)
    {
        if(!player.IsAuthorized())return;
        var business = BusinessHandler.GetBusinessById(businessId);
        if (business != null && business.BusinessType == BusinessesTypes.GasStation)
        {
            var item = business.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item.Count < 1)
            {
                player.SendNotify(NotifyType.Warning, "Это топливо закончилось");
                return;
            }

            var vehicle = player.Vehicle;
            if (vehicle == null && player.VehicleSeat != (int)VehicleSeat.Driver)
            {
                player.SendNotify(NotifyType.Error, "Вы должны быть в машине");
                return;
            }
            if (player.MinusMoney(item.Price * count))
            {
                player.ChangeCefWindow(CefWindowsPaths.Default);
                var modelVehicle = vehicle.GetVehicleModelFromDb();
                vehicle.SetVehicleIsRefueling(true);
                player.SendProgressBar((count*200)/1000, "Заправка...");
                NAPI.Task.Run(() =>
                {
                    VehicleHandler.AddFuel(modelVehicle,count);
                    vehicle.SetVehicleIsRefueling(false);
                },count*200);
                if (business.OwnerCharacterId != 0)
                {
                    BusinessHandler.RemoveItem(item, business, count);
                    BusinessHandler.AddMoneyInBank(item.Price * count, business.Id);
                }
                StatisticBusinessHandler.AddBuyProduct(business, count);
            }
        }
    }

    [RemoteEvent("CEF::SERVER:ON_BUY_CAR")]
    public void OnBuyCar(Player player, int businessId, int idItem, string hexColor)
    {
        if(!player.IsAuthorized())return;
        var business = BusinessHandler.GetBusinessById(businessId);
        if (business == null || business.BusinessType != BusinessesTypes.CarDealerShipLuxury)
        {
            return;
        }
        var item = business.Items.FirstOrDefault(i => i.ItemId == idItem);
        if (item.Count < 1)
        {
            player.SendNotify(NotifyType.Warning, "Эти машины закончились");
            return;
        }
        if (player.MinusMoney(item.Price))
        {
            var vehicleType = VehicleTypeHandler.GetVehicleTypeById(item.ItemId);
            uint vehHash = NAPI.Util.GetHashKey(vehicleType.ModelHash);
            var veh = NAPI.Vehicle.CreateVehicle(vehHash, new Vector3(-808.8914,-227.92992,36.73519), 30.815432f, 0 ,0);
            var color = System.Drawing.ColorTranslator.FromHtml(hexColor);
            veh.CustomPrimaryColor = new Color(color.R, color.G,color.B);
            veh.CustomSecondaryColor = new Color(color.R, color.G,color.B);
            veh.NumberPlate = VehicleService.GetUniqNumberPlate();
            veh.Locked = true;
            veh.EngineStatus = true;
            VehicleHandler.AddNewVehicle(player.GetCharacter(), vehicleType, veh);
            
            if (business.OwnerCharacterId != 0)
            {
                BusinessHandler.RemoveItem(item, business);
                BusinessHandler.AddMoneyInBank(item.Price, business.Id);
            }
            StatisticBusinessHandler.AddBuyProduct(business);
            InteriorService.ExitSoloInterior(player);
            player.SetIntoVehicle(veh, 0);
            player.ChangeCefWindow(CefWindowsPaths.Default);
        }
    }
}