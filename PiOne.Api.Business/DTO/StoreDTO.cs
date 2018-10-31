using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class StoreDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string ImageData { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string IndustryID { get; set; }
        public string IndustryName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string GSTRegNo { get; set; }
        public string TimeZone { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime LastModified { get; set; }
        public List<BusinessHourDTO> ListBusinessHour { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string StoreCode { get; set; }
        public string  CurrencySymbol { get; set; }
        public TaxDTO Tax { get; set; }                     /* get setting for Hang Flower App */
        public ServiceChargeDTO ServiceCharge { get; set; } /* get setting for Hang Flower App */
        public List<SettingDTO> ListSetting { get; set; }   /* get setting for Hang Flower App */

        public List<PromotionDTO> ListPromotion { get; set; } // Hang Flower App
        public List<ProductOnKioskDTO> ListProductPromo { get; set; } // Hang Flower App
        public List<ProductOnKioskDTO> ListArtist { get; set; } // Hang Flower App
        public List<SeasonDTO> ListSeason { get; set; }  // Hang Flower App
        public MerchantDTO MerchantPiOne { get; set; }
        public int MaximumNumberOfPerson { get; set; }
        public bool IsAllowBookingOnline { get; set; }


        public StoreDTO()
        {
            ListPromotion = new List<PromotionDTO>();
            ListArtist = new List<ProductOnKioskDTO>();
            ListProductPromo = new List<ProductOnKioskDTO>();
        }
    }

    public class MerchantDTO
    {
        public string ID { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Remarks { get; set; }
        public string ColorCode { get; set; }
        public bool IsFree { get; set; }
        public string ActiveCode { get; set; }
    }

    public class ArtistDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageURL { get; set; }

        public ArtistDTO()
        {
        }
    }

    public class GetListStoreDTO : NSApiResponseBase
    {
        public List<StoreDTO> ListStore { get; set; }
        public bool IsArtist { get; set; }

        public GetListStoreDTO()
        {
            ListStore = new List<StoreDTO>();
        }
    }

    public class ResponseGetListMerchantSearch : NSApiResponseBase
    {
        public List<MerchantDetailDTO> ListMerchantDTO { get; set; }

        public ResponseGetListMerchantSearch()
        {
            ListMerchantDTO = new List<MerchantDetailDTO>();
        }
    }
    public class MerchantDetailDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageURL { get; set; }
        public string ImageData { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ColorCode { get; set; }
        public bool IsRegisterOnline { get; set; }

        /* delivery setting */
        public bool IsAllowCreateDeliveryOnline { get; set; }
        public bool IsAllowCreatePickupOnline { get; set; }
        public List<string> ListPostalCodeDelivery { get; set; }

    }

}
