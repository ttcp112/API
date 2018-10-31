using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Business.DTO;
using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System.Threading.Tasks;

namespace PiOne.Api.Consumers.Business
{
    public class BUSMember : NSBusMember
    {
        protected static BUSMember _instance;

        public static BUSMember Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSMember();
                return _instance;
            }
        }

        static BUSMember()
        {
        }

        public async Task<NSApiResponse> MemberLogin(MemberLoginRequest input)
        {
            NSLog.Logger.Info("MemberLogin", input);
            return base.MemberLogin(input.Nickname, input.Password, input.DeviceToken, (byte)input.DeviceType, input.LanguageID, input.UUID);
        }

        public async Task<NSApiResponse> RegisterMember(RegisterMemberRequest input)
        {
            NSLog.Logger.Info("RegisterMember", input);
            return base.RegisterMember(input.Member, input.DeviceToken, (byte)input.DeviceType, input.LanguageID, input.UUID);
        }

        public async Task<NSApiResponse> ForgotPassword(ForgotPasswordMemberRequest input)
        {
            NSLog.Logger.Info("ForgotPasswordMember", input);
            return base.ForgotPassword(input.Nickname);
        }

        public async Task<NSApiResponse> ChangePassword(ChangePasswordMemberRequest input)
        {
            NSLog.Logger.Info("ChangePasswordMember", input);
            return base.ChangePassword(input.MemberID, input.OldPassword, input.NewPassword, input.ID);
        }

        public async Task<NSApiResponse> EditProfile(EditProfileMemberRequest input)
        {
            NSLog.Logger.Info("EditProfile", input);
            return base.EditProfile(input.Member);
        }

        public async Task<NSApiResponse> GetCustomerHangGift(GetCustomerHangGiftRequest input)
        {
            NSLog.Logger.Info("GetCustomerHangGift", input);
            return base.GetCustomerHangGift(input.MemberID, input.StoreID, input.MerchantID, input.PageSize, input.PageIndex, input.DateFrom, input.DateTo);
        }

        public async Task<NSApiResponse> AddEditMemberCard(MemberCardDTO input)
        {
            NSLog.Logger.Info("AddEditMemberCard", input);
            return base.AddEditMemberCard(input);
        }

        public async Task<NSApiResponse> GetMemberCard(RequestModelBase input)
        {
            NSLog.Logger.Info("GetMemberCard", input);
            return base.GetMemberCard(input.MemberID);
        }

        public async Task<NSApiResponse> RegisterArtist(RegisterArtistReqeust input)
        {
            NSLog.Logger.Info("RegisterArtist", input);
            return base.RegisterArtist(input.MemberID, input.SpecCode);
        }

        public async Task<NSApiResponse> ArtistCheckInOut(ArtistCheckInOutReqeust input)
        {
            NSLog.Logger.Info("ArtistCheckInOut", input);
            return base.ArtistCheckInOut(input.MemberID, input.StoreID);
        }

        public async Task<NSApiResponse> GetMember(GetMemberRequest input)
        {
            NSLog.Logger.Info("GetMember", input);
            return base.GetMember(input.MemberID);
        }

        public async Task<NSApiResponse> GetTier(RequestModelBase input)
        {
            NSLog.Logger.Info("GetTier", input);
            return base.GetTier();
        }

        public async Task<NSApiResponse> MemberLogout(RequestModelBase input)
        {
            NSLog.Logger.Info("MemberLogout", input);
            return base.MemberLogout(input.DeviceToken, (byte)input.DeviceType);
        }
        public async Task<NSApiResponse> MemberLoyaltyUpdate(RequestUpdateLoyalty input)
        {
            NSLog.Logger.Info("MemberLoyaltyUpdate", input);
            return base.MemberLoyaltyUpdate(input.MemberID, input.Tier, input.Point);
        }
        public async Task<NSApiResponse> MemberNoti(SendMessageRequest input)
        {
            NSLog.Logger.Info("MemberNoti", input);
            return base.MemberNoti(input.Message);
        }
        public async Task<NSApiResponse> SearchMember(RequestSearchMember input)
        {
            NSLog.Logger.Info("SearchMember", input);
            return base.SearchMember(input.MemberID, input.Info.Name, input.Info.Email, input.Info.IC, input.Info.Phone);
        }

        public async Task<bool> CheckData(int minutes)
        {
            NSLog.Logger.Info("CheckData - Delay: ", minutes);
            return base.CheckData(minutes);
        }
    }
}