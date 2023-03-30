using System.Text.Json.Serialization;

namespace Models
{
    public class MyOrder
    {
        [property: JsonPropertyName("myOrderId")]
        public int MyOrderId { get; set; }

        [property: JsonPropertyName("myOrderName")]
        public string MyOrderName { get; set; } = "avocado on rye";
    }
}