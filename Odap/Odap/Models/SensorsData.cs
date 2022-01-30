namespace Odap.Models
{
    public class SensorsData : BaseModel
    {
        public string UniqueDeviceId { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double? Alt { get; set; }
        public  double? AccuracyGPS { get; set; }
        public  string DirectionEastWest { get; set; }
        public  string DirectionNorthSouth { get; set; }
        public double? Accuracy { get; set; }
        public float AccX { get; set; }
        public float AccY { get; set; }
        public float AccZ { get; set; }
        public float RSSI { get; set; }
    }
}
