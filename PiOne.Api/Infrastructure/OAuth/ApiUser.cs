using PiOne.Api.Infrastructure.Interfaces;

namespace PiOne.Api.Infrastructure.OAuth
{
    public class ApiUser : IUser
    {
        public string Name { get; set; }
        public string OrganisationId { get; set; }
    }
}