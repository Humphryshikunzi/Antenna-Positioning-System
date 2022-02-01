using System;

namespace OdapApi.Models.Device
{
    public class DeviceData : BaseModel
    {
        public string UniqueDeviceId { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double? Alt { get; set; }
        public double? AccuracyGPS { get; set; }
        public string DirectionEastWest { get; set; }
        public string DirectionNorthSouth { get; set; }
        public double? Accuracy { get; set; }
        public float AccX { get; set; }
        public float AccY { get; set; }
        public float AccZ { get; set; }
        public float RSSI { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }        
    }
}
