using PiOne.Api.Business.DTO;
using PiOne.Api.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PiOne.Api.Consumers.RequestDTO
{
    public class OrderRequest
    {
    }

    public class RequestOrderCreate : RequestModelBase
    {
        public string TableID { get; set; }
    }

    public class RequestOrderCheckDrawer : RequestModelBase
    {
    }

    public class RequestOrderDelete : RequestModelBase
    {
    }

    public class RequestOrderAddItem : RequestModelBase
    {
        public AddItemToOrderDTO AddItemDTO { get; set; }
    }

    public class RequestOrderGetDetail : RequestModelBase
    {
        /* 1poins have 1 order, don't care storeID */
        //public string StoreID { get; set; } 
    }

    public class RequestOrderChangeStateTable : RequestModelBase /* change state to call for bill or call for service */
    {
        public int State { get; set; } /* use enum ETableStatus from nupos */
    }

    public class RequestOrderPayByExternal : RequestModelBase
    {
        public string PaymentMethodID { get; set; }
        public double Amount { get; set; }
    }

    public class RequestOrderDone : RequestModelBase
    {
    }

    public class RequestOrderSync : RequestModelBase
    {
        public OrderDetailDTO OrderData { get; set; }
    }
    public class DoneTempOrderRequest : RequestModelBase
    {
        public string OrderID { get; set; }
        public string PaymentMethodID { get; set; }
        public double Amount { get; set; }
    }
}