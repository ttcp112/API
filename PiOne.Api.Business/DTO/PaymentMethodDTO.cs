using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
#region -- response
    public class ResponsePaymentMethodGetExternal : NSApiResponseBase
    {
        public List<PaymentMethodDTO> ListPaymentMethod { get; set; }
    }

#endregion
    public class PaymentMethodDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public bool IsActive { get; set; }
        public bool IsHasConfirmCode { get; set; }
        public int NumberOfCopies { get; set; }
        public double FixedAmount { get; set; }
        public bool IsGiveBackChange { get; set; }
        public bool IsAllowKickDrawer { get; set; }
        public bool IsIncludeOnSale { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public byte Code { get; set; }
        public string GLAccountCode { get; set; }
        public bool IsWithTerminal { get; set; }
        public string ThirdPartyID { get; set; }
        public byte TerminalType { get; set; }
        public string Index { get; set; } /* Index of payment method in excel file when importing payment method */
        public bool IsShowOnPos { get; set; }
        public List<PaymentMethodDTO> ListChild { get; set; }
        
        /* external info */
        public PaymentGateWayDTO PaymentGateWayData { get; set; }
    }

    public class PaymentGateWayDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public byte PayeeMode { get; set; }
        public string ThirdPartyID { get; set; }
        public string PGConfigurationID { get; set; }
        public List<ParameterDTO> ListParameter { get; set; }
    }

    public class ParameterDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
