namespace L4D2PlayStats.Core.GameInfo.Results;

public class SendExternalMessageResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public static SendExternalMessageResult SuccessResult()
    {
        return new SendExternalMessageResult { Success = true };
    }

    public static SendExternalMessageResult FailureResult(string errorMessage)
    {
        return new SendExternalMessageResult { Success = false, ErrorMessage = errorMessage };
    }
}