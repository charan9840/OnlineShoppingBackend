namespace OnlineShoppingBackend.Models
{
    public class StatusMessage
    {
        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public object Value { get; set; }
    }
}
