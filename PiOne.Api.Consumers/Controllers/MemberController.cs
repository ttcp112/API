using PiOne.Api.Consumers.Business;
using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Controller;
using PiOne.Api.Core.Response;
using PiOne.Api.Consumers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PiOne.Api.Core.Request;
using PiOne.Api.Business.DTO;

namespace PiOne.Api.Consumers.Controllers
{
    public class MemberController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Member_Login)]
        public async Task<NSApiResponse> MemberLogin(MemberLoginRequest request)
        {
            return await BUSMember.Instance.MemberLogin(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Register)]
        public async Task<NSApiResponse> RegisterMember(RegisterMemberRequest request)
        {
            return await BUSMember.Instance.RegisterMember(request);
        }

        [HttpPost, Route(ApiRoutes.Member_ForgotPassword)]
        public async Task<NSApiResponse> ForgotPassword(ForgotPasswordMemberRequest request)
        {
            return await BUSMember.Instance.ForgotPassword(request);
        }

        [HttpPost, Route(ApiRoutes.Member_ChangePassword)]
        public async Task<NSApiResponse> ChangePassword(ChangePasswordMemberRequest request)
        {
            return await BUSMember.Instance.ChangePassword(request);
        }

        [HttpPost, Route(ApiRoutes.Member_EditProfile)]
        public async Task<NSApiResponse> EditProfile(EditProfileMemberRequest request)
        {
            return await BUSMember.Instance.EditProfile(request);
        }

        [HttpPost, Route(ApiRoutes.Member_GetCustomerHangGift)]
        public async Task<NSApiResponse> GetCustomerHangGift(GetCustomerHangGiftRequest request)
        {
            return await BUSMember.Instance.GetCustomerHangGift(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Card_AddEdit)]
        public async Task<NSApiResponse> AddEditMemberCard(AddEditMemberCardRequest request)
        {
            return await BUSMember.Instance.AddEditMemberCard(request.Card);
        }

        [HttpPost, Route(ApiRoutes.Member_Card_Get)]
        public async Task<NSApiResponse> GetMemberCard(RequestModelBase request)
        {
            return await BUSMember.Instance.GetMemberCard(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Artist_Register)]
        public async Task<NSApiResponse> RegisterArtist(RegisterArtistReqeust request)
        {
            return await BUSMember.Instance.RegisterArtist(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Artist_CheckInOut)]
        public async Task<NSApiResponse> ArtistCheckInOut(ArtistCheckInOutReqeust request)
        {
            return await BUSMember.Instance.ArtistCheckInOut(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Get)]
        public async Task<NSApiResponse> GetMember(GetMemberRequest request)
        {
            return await BUSMember.Instance.GetMember(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Get_Tier)]
        public async Task<NSApiResponse> GetTier(RequestModelBase request)
        {
            return await BUSMember.Instance.GetTier(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Logout)]
        public async Task<NSApiResponse> MemberLogout(RequestModelBase request)
        {
            return await BUSMember.Instance.MemberLogout(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Loyalty_Update)]
        public async Task<NSApiResponse> MemberLoyaltyUpdate(RequestUpdateLoyalty request)
        {
            return await BUSMember.Instance.MemberLoyaltyUpdate(request);
        }

        [HttpPost, Route(ApiRoutes.Member_Loyalty_Noti)]
        public async Task<NSApiResponse> MemberLoyaltyNoti(SendMessageRequest request)
        {
            return await BUSMember.Instance.MemberNoti(request);
        }


        [HttpPost, Route(ApiRoutes.Member_Search)]
        public async Task<NSApiResponse> MemberSearch(RequestSearchMember request)
        {
            return await BUSMember.Instance.SearchMember(request);
        }

        [HttpGet, Route(ApiRoutes.Member_Check_Data)]
        public async Task<bool> CheckData(int minutes = 5)
        {
            return await BUSMember.Instance.CheckData(minutes);
        }
    }
}