namespace Orders.Shared.Responses
{
    public class ActionResponse<T>
    {
        public bool WasSuccesse { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
    }
}
