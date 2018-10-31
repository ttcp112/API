using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class MemberDTO
    {
        public string ID { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string ImageURL { get; set; }
        public string ImageData { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string IC { get; set; }
        public string Passport { get; set; }
        public int Points { get; set; }
        public string MemberTier { get; set; }
        public string MemberTierColor { get; set; }
        public string MemberTierImage { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public bool IsMarried { get; set; }
        public DateTime? Anniversary { get; set; }
        public DateTime? MemberSince { get; set; }
        public string HomeAddress { get; set; }
        public string HomeCity { get; set; }
        public string HomeCountry { get; set; }
        public string HomePostalCode { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeCountry { get; set; }
        public string OfficePostalCode { get; set; }
        public string Remarks { get; set; }
        public bool IsArtist { get; set; }
        public bool IsPDPA { get; set; }
    }

    public class ResponseGetMember : NSApiResponseBase
    {
        public MemberDTO Member { get; set; }
    }

    public class ResponseMemberLogin : NSApiResponseBase
    {
        public MemberDTO Member { get; set; }
        public string SocketPort { get; set; }
        public int NumOfNewMessageChatting { get; set; } // sum message
        public int NumOfNewMessage { get; set; } // sum customer
        public int NumOfBottleService { get; set; }
        public int NumOfUpcomingBooking { get; set; }
        public List<MemberLocationDTO> ListLocation { get; set; }

        public ResponseMemberLogin()
        {
            Member = new MemberDTO();
            ListLocation = new List<MemberLocationDTO>();
            NumOfBottleService = 0;
            NumOfNewMessage = 0;
        }
    }

    public class ResponseGetCustomerHangGift : NSApiResponseBase
    {
        public List<CustomerDTO> ListCustomer { get; set; }
    }

    public class MemberCardDTO
    {
        public string ID { get; set; }
        public string MemberID { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CSV { get; set; }
    }
    public class ResponseGetMemberCard : NSApiResponseBase
    {
        public List<MemberCardDTO> ListCard { get; set; }
    }

    public class IntegrateArtistDTO
    {
        public string ID { get; set; }
        public string LoginID { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }

    public class RegisterArtistRequestIntegrate
    {
        public IntegrateArtistDTO Artist { get; set; }
        public string SpecCode { get; set; }
    }

    public class ArtistCheckInOutRequestIntegrate
    {
        public IntegrateArtistDTO Artist { get; set; }
        public string StoreID { get; set; }
    }

    public class RegisterArtistReqeust : RequestModelBase
    {
        public string SpecCode { get; set; }
    }

    public class ArtistCheckInOutReqeust : RequestModelBase { }

    public class ArtistCheckInOutResponse : NSApiResponseBase
    {
        public bool IsCheckIn { get; set; }
    }

    public class GetMemberRequest
    {
        public string MemberID { get; set; }
    }

    public class GetCustomerHangGiftRequest : RequestModelBase
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public GetCustomerHangGiftRequest()
        {
            DateFrom = DateTime.MinValue;
            DateTo = DateTime.MinValue;
        }
    }
    public class ITierDTO
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Img { get; set; }
        public int Point { get; set; }
        public int Code { get; set; }
    }

    public class ResponseGetTier : NSApiResponseBase
    {
        public List<ITierDTO> ListTier { get; set; }
        public ResponseGetTier()
        { ListTier = new List<ITierDTO>(); }
    }

    public class RequestUpdateLoyalty : RequestModelBase
    {
        public ITierDTO Tier { get; set; }
        public int Point { get; set; }
    }
    public class RequestSearchMember : RequestModelBase
    {
        public CustomerDTO Info { get; set; }
    }

}
