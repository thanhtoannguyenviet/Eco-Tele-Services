using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace Client.Models
{
    public static class PayPalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Static constructor for setting the readonly static members.
        static PayPalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            var config = new Dictionary<string, string>();
            config.Add("mode", "sandbox");
            config.Add("connectionTimeout", "360000");
            config.Add("requestRetries", "1");
            config.Add("clientId", "AXqEeQflkTgo7erdKlnnizY1bx3TY1ds46D9OJFKNhap9ReFF2mWnWA_4gqRbX3bXTwYPu8gYon3e02P");
            config.Add("clientSecret", "EO-TNSIC_HeHo8iNW5t6jAqjy83ZinTmuTRppPXfc8mrIJrKZykOvWXiSgh-i0-O2fRNcEtuYWDDWqpr");
            return config;
        }

        
        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext(string accessToken = "", string requestID = "")
        {
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken, string.IsNullOrEmpty(requestID) ? Guid.NewGuid().ToString() : requestID);
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}