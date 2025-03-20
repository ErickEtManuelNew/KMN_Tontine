namespace KMN_Tontine.Blazor.UI.Services.Base
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
    }
} 