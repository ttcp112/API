using System;

namespace PiOne.Api.Common
{
    public class Constants
    {
        public const string IVARC = "INVALID_APP_REGISTERED_CODE";
        public const string CNEX = "COMPANY_DOES_NOT_EXIST";

        public const string AKASNF = "AppKey/AppSecret was not found.";
        public const string AHNBR = "App has not been registered. Contact Service Provider for more information.";

        public const string DUPL = "Dupplicate value or data already exist in the system.";

        //=========================================================
        //General setting
        public const string SwitchOn = "true";

        public const string SwitchOff = "false";

        public const string Male = "Male";
        public const string Female = "Female";
        public const string Currency = "Currency";
        public static DateTime MinDate = new DateTime(1900, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public static DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        public const string GSTTax = "GST";
        public const string Cash = "Cash";
        public const string GiftCard = "Gift Card";

        //report title
        public const string ReportTitle_Receipt = "RECEIPT";

        public const string ReportTitle_AuditReceipt = "AUDIT RECEIPT";

        public const string PasswordChar = "abcdefghijklmnopqrstuvwxyz0123456789";
        public const string QRNULL = "Invalid Wallet QR Code/ IC Code";
        public const string QRMerchantNull = "Customer not existed in Merchant";

        public enum EGender
        {
            Male = 1,
            Female = 0
        }

        public enum EState
        {
            NotSend = 0,
            Send = 1
        }

        public enum EStatus
        {
            Actived = 1,
            Deleted = 9,
            InActived = 3,
            Demo = 2,
            Refund = 4
        }

        public enum EValueType
        {
            Percent = 0,
            Currency = 1,
        }

        public enum ETax
        {
            TaxExempt = 1,
            Inclusive = 2,
            AddOn = 3
        }

        public enum ESettingCode
        {
            Background = 1,
            InsertMessage = 2,
            MessageSignature = 3,
            InsertImage = 4,
            ImageSignature = 5,
            DuplicateBooking = 6,
            NumberOfDuplicates = 7,
            BookingType = 8,
            TimeSlot = 9,               /* Time slot setting for booking */
            TimeHoldBefore = 10,         /* Time hold before for booking */
            TimeHoldAfter = 11,          /* Time hold after for booking */
            MaxNoOfPerson = 12,
            TimeValidBooking = 13,       /* Time valid for booking */
            TimeRemindReservation = 14,
            AddOnCoverNo = 15,           /* Number add to cover No */
            BookingOnline = 16,
            RechargeGiftCard = 17,
            ValueRechargeGiftCard = 18,
            MakeEarning = 19,
            ValueMakeEarning = 20,
            BuyGiftCard = 21,
            ValueBuyGiftCard = 22,
            OrderReceiptChecking = 23,
            Email = 24,
            Password = 25,
            TimeRemindPromoExpiry = 26,
            TimeRemindGiftCardExpiry = 27,
            WalletPDPA = 28,
            TermsConditions = 29,
            DefaultServiceTime = 30,
            GCHeader = 31,
            GCFooter = 32,
            SelfOrdering_PayLater = 33,
            SelfOrdering_PayGiftCard = 34,
            SelfOrdering_PayExternalTerminal = 35,
            Delivery_PayLater = 36,
            Delivery_PayGiftCard = 37,
            Delivery_PayExternalTerminal = 38,
            Pickup_PayLater = 39,
            Pickup_PayGiftCard = 40,
            Pickup_PayExternalTerminal = 41,
            EatIn_PayLater = 42,
            EatIn_PayGiftCard = 43,
            EatIn_PayExternalTerminal = 44,
            Tier = 45,
        }

        public enum EPaymentCode
        {
            Parent = 0,
            Cash = 1,
            Redeem = 2,
            External = 3,
            GiftCard = 4
        }

        public enum EReservationStatus
        {
            All = 0,
            Confirm = 1,
            ReConfirm = 2,
            Remind = 3,
            Checkin = 4, /* is occupie on nupos */
            Paid = 5,
            Cancel = 6,
            NotCome = 7,
        }

        public enum EOperatorType /* Use to get operator of Earning Rule & Spending Rule */
        {
            All = 0,
            And = 1,    /* operator and */
            Or = 2      /* operator or */
        }

        public enum ESpendType  /* Type of spend Rule */
        {
            BuyItem = 1,
            SpendMoney = 2,
        }

        public enum ESpendOnType /* Spend on Type */
        {
            AnyItem = 3,
            SpecificItem = 4,
            Category = 5,
            TotalBill = 6,              /* for earn rule */
            SingleReceipt = 7,          /* for memmbership rule */
            AccumulativeReceipt = 8     /* for memmbership rule */
        }

        public enum EEarnType   /* Earn rule apply on type */
        {
            TotalBill = 1,
            SpentItem = 2,
            SpecificItem = 3,   /* Earn type is specific Item => earn on type is item   */
            Category = 4,       /* Earn type is category => earn on type is item        */
            Point = 5,          /* Earn type is point => don't have earn on type        */
            Money = 6,          /* User for Redeem type */
        }

        public enum ERepeatType
        {
            EveryDay = 1,
            DayOfWeek = 2,
            DayOfMonth = 3
        }

        public enum EFunctionCode
        {
            POSLite = 100,
            RegisterRenewMember = 101,
            Promotions = 102,
            Earning = 103,
            Redemption = 104,
            GiftCard = 105,
            TransactionApproved = 106,
            Reservation = 200,
            Activities = 300,
            PromotionsManagement = 400,
            CustomersManagement = 500,
            MembershipManagement = 600,
            GiftCardManagement = 700,
            GiftCardList = 701,
            AccountManagement = 702,
            Settings = 800,
            DetailedInformation = 801,
            General = 802,
            iPad = 803,
            Theme = 804,
            Currency = 805,
            RoleSettings = 806,
            Printers = 807,
            Employees = 808,
            Services = 809,
            TableList = 810
        }

        public enum ELoginType
        {
            Manual = 0,
            Google = 1,
            Facebook = 2
        }

        public enum EGiftCardType
        {
            Both = 0,
            Value = 1,
            Item = 2
        }

        public enum EBookingRequestType
        {
            All = 0,
            Upcoming = 1,
            Past = 2,
        }

        public enum EMerchantType
        {
            History = 1,
            Favourite = 2,
            Membership = 3,
            Confirm = 4
        }

        public enum EIndustry
        {
            All = 0,
            FnB = 1,
            Beauty = 2,
        }

        /* ERedemptionType use to get redemption transaction */
        /* don't use ERedemptionType format redeem rule, EEarnType will format redeem rule */

        public enum ERedemptionType
        {
            TotalBill = 1,
            Item = 2,
        }

        public enum EAwardType
        {
            TotalBill = 1,
            Item = 2,
            Point = 3,
            Renew = 4
        }

        public enum EHistoryType
        {
            Redemption = 1,
            Earned = 2
        }

        public enum EProductType
        {
            All = 0,
            Dish = 1,
            Modifier = 2,
            Service = 3,
            SetMenu = 4,
            Discount = 5,
            Promotion = 6,

            //7
            GiftCard = 8,

            Misc = 9,
            SpecialModifier = 10
        }

        public enum EModifierType /* use for fied modifiertype of table orderDetail */
        {
            Product = 0,
            Forced = 1,
            Optional = 2,
            AdditionalModifier = 3,
            Special = 4,
            AdditionalDish = 5,
            GiftCardBuy = 8,
            GiftCardRecharge = 9,
            GiftCardReturn = 10,
            Gift = 11, /* use to know gift */
        }

        public enum EBookingType
        {
            TimeSlot = 1,
            ServiceTime = 2
        }

        public enum EPrinterSettingState
        {
            None = 0,
            Primary = 1,
            Secondary = 2
        }

        public enum EPrinterSettingType
        {
            Receipt = 1
        }

        public enum ERenewType /* use for Membership tier */
        {
            Point = 1,
            Money = 2,
        }

        public enum EGiftCardStatus
        {
            Active = 1,
            Inactive = 2,
            NotAvailable = 3,
            Expired = 4,
            Blacklist = 5
        }

        public enum EExpiredType
        {
            Never = 0,
            SpecificDay = 1,
            PeriodOfTime = 2
        }

        public enum EGiftCardTransType
        {
            All = 0,
            Recharge = 1,
            Payment = 2,
            Buy = 3,
            Redeem = 4,
            Refund = 5,
        }

        public enum EScanType
        {
            Normal = 0,
            Promotion = 1,
            Earning = 2,
            GiftCardBuy = 3,
            GiftCardPay = 4,
            GiftCardRecharge = 5,
            Redeem = 6,
            NonMembership = 7,
            OrderFromWallet = 8,        /* this order is make from wallet */
            Delivery_Pickup = 9,        /* this order is make from wallet with type = delivery/pickup */
            EatIn = 10
        }

        public enum EMessageType
        {
            Chat = 1,
            Gift = 2,
            Membership = 3,
            Bottle = 4,
            Booking = 5,
        }

        public enum EPoinsUserType
        {
            NotTheCustomer = 0,
            Customer = 1,
            Membership = 2
        }

        public enum EActionMerchant
        {
            Remove = 0,
            Follow = 1,
            Unfollow = 2,
            Confirm = 3
        }

        public enum EEarnRulesEvent     /* User for Earn Rule */
        {
            Normal = 1,
            Birthday = 2,
            Anniversary = 3
        }

        public enum EEarnRulesPeriod    /* User for Earn Rule */
        {
            Day = 1,
            Week = 2,
            Month = 3
        }

        public enum EScanResponseType
        {
            IsInvalid = 1,          /* Promotion is invalid */
            IsSaved = 2,            /* Promotion was saved */
            IsNotSaved = 3,         /* Promotion is not saved */
        }

        public enum EPromotionType  /* Type of promotion */
        {
            View = 1,
            Download = 2,
            Share = 3,
            DownloadAndShare = 4,
        }

        public enum EGroupType
        {
            Force = 1,
            Optional = 2,
            Additional = 3
        }

        public enum EDeviceType
        {
            iOS = 0,
            Android = 1,
            WindowPhone = 2,
        }

        public enum EMemberValidType
        {
            Normal = 0,
            LastTransaction = 1
        }

        public enum EThirdPartyType
        {
            Delivery = 0,
            Payment = 1,
            NuPos_Pozz = 2,
            //Pozz = 3
        }

        public enum ETerminalType
        {
            Net = 0,
            Credit = 1,
            Debit = 2,
            RSVP = 3,
            RSVPDis10 = 4,
            ApplePay = 5
        }

        public enum ERuleType /* enum use to know type of rule */
        {
            All = 0,
            MembershipRule = 1,
            EarningRule = 2,
            RedeemRule = 3,
            PromotionRule = 4,
        }

        public enum EBodyType
        {
            Text = 0,
            Currency = 1,
            Point = 2,
            Item = 3,
            QRCode = 4,
            Date = 5,
            MultiUse = 6,
        }

        public enum EGiftCardItemType
        {
            Default = 0,
            Redeem = 1,
        }

        public enum EGiftCardRechargeType
        {
            Input = 0,
            List = 1,
        }

        public enum EOnTierType
        {
            GiftCard = 0,
            Promotion = 1,
        }

        public enum EEventType
        {
            Normal = 0,
            Redeem = 1,
            Birthday = 2,
            Anniversary = 3,
        }

        public enum EBookingMethodType
        {
            Manual = 1,
            Integrate = 2,
        }

        public enum EDataPrintType
        {
            Earning = 0,
            Redeem = 1,
            BuyGiftCard = 2,
            PayGiftCard = 3,
            RechargeGiftCard = 4,
            Renew = 5,
            Promotion = 6,
            PrintGiftCard = 7,
            PrintMembership = 8,
            Unknow = 100,
            Receipt = 101 // To Print PrintReceiptPosDTO
        }

        public enum EDeliveryState
        {
            New = 1,
            Accepted = 2,
            AcceptedAndSend = 3,
            Pickup = 4,
            Completed = 5,
            Rejected = 6,
            Cancel = 7,
        }

        public enum EDeliveryType
        {
            Both = 0,
            Deliver = 1,
            SelfCollect = 2
        }

        public enum EDeliveryPayment
        {
            Both = 0,
            Paid = 1,
            COD = 2
        }

        public enum EAssetType
        {
            Product = 1,
            Function = 2
        }

        public enum EAssetState
        {
            Active = 0,
            Expired = 1,
            Block = 2,
            Remove = 9
        }

        public enum EAdditionType
        {
            Account = 5,
            Function = 6 //Module
        }

        public enum EChatTemplate
        {
            Artist = 1,
            Customer = 2,
        }

        public enum EOrderState
        {
            New = 1,
            Closed = 2,
            AddToTab = 3,
            PayToWaiter = 4,
            //PayByGift = 5,
        }

        public enum EBottlePushNoti
        {
            CheckIn = 1,
            CheckOut = 2,
            Remind = 3,
            Expire = 4,
            Dispose = 5,
        }
    }
}