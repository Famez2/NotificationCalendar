namespace NotificationCalendar.Api.Contracts;

public class ApiResponseV1<T>
{
    public T? Data { get; set; } = default;

    public ApiResponseV1.ResponseErrorModel? Error { get; set; }
}

public class ApiResponseV1 : ApiResponseV1<object>
{
    public class ResponseErrorModel
    {
        public string? Message { get; set; }

        public int StatusCode { get; set; }

        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
