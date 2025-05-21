namespace Network
{
    public class ApiRequestResult <T>
    {
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}