namespace L4D2PlayStats.Core.ExternalChat.Results;

public class SendMessageResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public static SendMessageResult SuccessResult()
    {
        return new SendMessageResult { Success = true };
    }

    public static SendMessageResult FailureResult(string errorMessage)
    {
        return new SendMessageResult { Success = false, ErrorMessage = errorMessage };
    }
}