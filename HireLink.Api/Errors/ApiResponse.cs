﻿
namespace HireLink.Api.Errors;

public class ApiResponse(int statusCode, string message)
{
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
}
