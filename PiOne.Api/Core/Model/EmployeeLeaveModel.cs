using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Core.Model
{
    public partial class EmployeeLeaveModel
    {
        public static Reason Work = new Reason() { Name = "Work", Symbol = "Work" };
        
        public static Reason Off = new Reason() { Name = "Off", Symbol = "Off" };
        
        public static Reason Annual = new Reason() { Name = "Annual Leave (AL)", Symbol = "AL" };
        
        public static Reason Medical = new Reason() { Name = "Medical Leave (ML)", Symbol = "ML" };
        
        public static Reason NoPay = new Reason() { Name = "No Pay Leave (NPL)", Symbol = "NPL" };

        public static Reason Other = new Reason() { Name = "Other Leave (OL)", Symbol = "OL" };
    }

    public class Reason
    {
        public string Name { get; set; }

        public string Symbol { get; set; }
    }

}
