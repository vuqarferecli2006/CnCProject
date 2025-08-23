using System.Net;

namespace CnC.Application.Shared.Responses;

public class ResetPasswordModelResponse
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Message { get; set; } = null!;
    public HttpStatusCode StatusCode { get; set; }
    public bool Success {  get; set; }


    public ResetPasswordModelResponse(string message,HttpStatusCode statuscode)
    {
        Message = message;
        StatusCode = statuscode;
        Success = false;
    }
    public ResetPasswordModelResponse(string email,string token,string message,bool isSucces,HttpStatusCode statuscode)
    {
        Email = email;
        Token = token;
        Message = message;
        Success = isSucces;
        StatusCode = statuscode;
    }
}
