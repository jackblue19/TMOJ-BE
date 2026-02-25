namespace WebAPI.Models.Common;

public sealed record ApiResponse<T>(T data ,
                                   string? Message = null ,
                                   string? TraceId = null)
{
    public static ApiResponse<T> Ok(T data , string? message , string? TraceId = null)
        => new(data , message , TraceId);
}

