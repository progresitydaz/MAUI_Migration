using System;
namespace StembureauApp.Xamarin.Core.Models
{
    public class AuthenticationTokenResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public long ExpiresIn { get; set; }
        public string Scope { get; set; }
        public Guid Jti { get; set; }
    }
}
