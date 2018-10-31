using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PiOne.Api.Business.Consumer.Business;

namespace PiOne.Api.Consumers.Business
{
    public class BUSOrder : NSBusOrder
    {
        protected static BUSOrder _instance;
        public static BUSOrder Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSOrder();
                return _instance;
            }


        }
        static BUSOrder() { }

        public async Task<NSApiResponse> OrderCreate(RequestOrderCreate input)
        {
            NSLog.Logger.Info("OrderCreate", input);
            return base.OrderCreate(input.StoreID, input.TableID, input.MemberID, input.LanguageID);
        }

        public async Task<NSApiResponse> OrderCheckDrawer(RequestOrderCheckDrawer input)
        {
            NSLog.Logger.Info("OrderCheckDrawer", input);
            return base.OrderCheckDrawer(input.ID, input.LanguageID);
        }

        public async Task<NSApiResponse> OrderDelete(RequestOrderDelete input)
        {
            NSLog.Logger.Info("WalletOrderDelete", input);
            return base.OrderDelete(input.ID);
        }

        public async Task<NSApiResponse> OrderAddItem(RequestOrderAddItem input)
        {
            NSLog.Logger.Info("OrderAddItem", input);
            return base.OrderAddItem(input.AddItemDTO, input.StoreID, input.MemberID);
        }

        public async Task<NSApiResponse> OrderGetDetail(RequestOrderGetDetail input)
        {
            NSLog.Logger.Info("OrderGetDetail", input);
            return base.OrderGetDetail(input.StoreID, input.MemberID, input.ID);
        }

        public async Task<NSApiResponse> OrderChangeStateTable(RequestOrderChangeStateTable input)
        {
            NSLog.Logger.Info("OrderChangeStateTable", input);
            return base.OrderChangeStateTable(input.StoreID, input.ID, input.State);
        }

        public async Task<NSApiResponse> OrderPayByExternal(RequestOrderPayByExternal input)
        {
            NSLog.Logger.Info("OrderPayByExternal", input);
            return base.OrderPayByExternal(input.StoreID, input.MemberID, input.ID, input.PaymentMethodID, input.Amount);
        }

        public async Task<NSApiResponse> OrderDone(RequestOrderDone input)
        {
            NSLog.Logger.Info("OrderDone", input);
            return base.OrderDone(input.StoreID, input.ID);
        }

        public async Task<NSApiResponse> OrderSync(RequestOrderSync input)
        {
            NSLog.Logger.Info("OrderSync", input);
            return base.OrderSync(input.OrderData);
        }

        public async Task<NSApiResponse> OrderTempDone(DoneTempOrderRequest input)
        {
            NSLog.Logger.Info("OrderTempDone", input);
            return base.OrderDoneTemp(input.OrderID, input.StoreID, input.PaymentMethodID, input.Amount);
        }
    }
}