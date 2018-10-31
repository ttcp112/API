using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class TaxDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double Percent { get; set; }
        public int TaxType { get; set; }
        public int NoOfProduct { get; set; }
        public string Index { get; set; }       /* Index of tax in excel, use for importing function */
        public string StoreID { get; set; }     /* Tax in store level */
        public string StoreName { get; set; }
    }
}
