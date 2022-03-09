using OdapApi.Models;
using System;

namespace Odap.Models
{
    public class RAngle : BaseModel
    {
        public  int  Angle { get; set; }
        public  DateTime  DateReceived { get; set; }
        public  bool  IsApproved { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string EmailToSendTo { get; set; } 
    }
}
