using PiOne.Api.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class CustomerDTO
    {
        public string ID { get; set; }
        public string MemberID { get; set; }
        public string IC { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string ImageData { get; set; } /* Use to update or insert new image */
        public bool IsActive { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public bool Marital { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime Anniversary { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsMembership { get; set; }
        public bool IsWantMembership { get; set; }
        public string HomeStreet { get; set; }
        public string HomeCity { get; set; }
        public string HomeZipCode { get; set; }
        public string HomeCountry { get; set; }
        public string OfficeStreet { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeZipCode { get; set; }
        public string OfficeCountry { get; set; }
        public double TotalPaidAmout { get; set; }
        public double ByCash { get; set; }
        public double ByExTerminal { get; set; }
        public double ByGiftCard { get; set; }
        public double TotalRefund { get; set; }
        public DateTime LastVisited { get; set; }
        public int Reservation { get; set; }
        public int Cancelation { get; set; }
        public int WalkIn { get; set; }
        public string Index { get; set; } /* Index of Customer in excel when importing customer */
        public string PoinsID { get; set; }
        public string CompanyReg { get; set; }

        public List<MembershipLiteDTO> ListStore { get; set; }
        public DateTime HangDate { get; set; }
        public bool IsNewMess { get; set; }
        public List<TableLiteDTO> ListTable { get; set; }

        public CustomerDTO()
        {
            ListStore = new List<MembershipLiteDTO>();
            ListTable = new List<TableLiteDTO>();
        }
    }

    public class TableLiteDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class MembershipLiteDTO
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public bool IsMembership { get; set; }
    }
}
