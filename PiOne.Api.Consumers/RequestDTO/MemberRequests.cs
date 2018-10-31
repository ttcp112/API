using PiOne.Api.Business.DTO;
using PiOne.Api.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PiOne.Api.Consumers.RequestDTO
{
    public class MemberRequests
    {
    }
    public class MemberLoginRequest : RequestModelBase
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
    }

    public class RegisterMemberRequest : RequestModelBase
    {
        public MemberDTO Member { get; set; }
    }

    public class ForgotPasswordMemberRequest : RequestModelBase
    {
        public string Nickname { get; set; }
    }

    public class ChangePasswordMemberRequest : RequestModelBase
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class EditProfileMemberRequest : RequestModelBase
    {
        public MemberDTO Member { get; set; }
    }
    public class AddEditMemberCardRequest : RequestModelBase
    {
        public MemberCardDTO Card { get; set; }
    }
}