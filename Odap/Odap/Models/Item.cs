namespace Odap.Models
{
    public class Item : BaseModel
    {
        public string UniqueItemKey { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public float  Value { get; set; }
        public  string  Unit { get; set; }
    }
}