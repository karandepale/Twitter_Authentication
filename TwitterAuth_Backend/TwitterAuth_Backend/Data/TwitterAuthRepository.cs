using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterAuth_Backend.Model;
using OAuth;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TwitterAuth_Backend.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TwitterAuth_Backend.Data
{
    public class TwitterAuthRepository : ITwitterAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<TwitterSettings> _twitterConfig;
        private readonly TwitterContext context;

        public TwitterAuthRepository(
           IConfiguration config,
           IHttpClientFactory clientFactory,
           IOptions<TwitterSettings> twitterConfig,
           TwitterContext context
           )               
        {
            _twitterConfig = twitterConfig;
            _clientFactory = clientFactory;
            _config = config;
           this.context = context;
        }


        //GET REQUEST TOKEN:-
        public async Task<RequestTokenResponse> GetRequestToken()
        {

            var requestTokenResponse = new RequestTokenResponse();


            var client = _clientFactory.CreateClient("twitter");
            var consumerKey = _twitterConfig.Value.AppId;
            var consumerSecret = _twitterConfig.Value.AppSecret;
            var callbackUrl = "http://localhost:4200/Login";

            client.DefaultRequestHeaders.Accept.Clear();

            var oauthClient = new OAuthRequest
            {
                Method = "POST",
                Type = OAuthRequestType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                RequestUrl = "https://api.twitter.com/oauth/request_token",
                Version = "1.0a",
                Realm = "twitter.com",
                CallbackUrl = callbackUrl
            };

            string auth = oauthClient.GetAuthorizationHeader();

            client.DefaultRequestHeaders.Add("Authorization", auth);



            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(oauthClient.RequestUrl, content))
                {
                    response.EnsureSuccessStatusCode();

                    var responseString = response.Content.ReadAsStringAsync()
                                               .Result.Split("&");


                    requestTokenResponse = new RequestTokenResponse
                    {
                        oauth_token = responseString[0],
                        oauth_token_secret = responseString[1],
                        oauth_callback_confirmed = responseString[2]
                    };


                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return requestTokenResponse;

        }


        //Get Access Token
        public async Task<UserModelDto> GetAccessToken(string token, string oauthVerifier)
        {
            var client = _clientFactory.CreateClient("twitter");
            var consumerKey = _twitterConfig.Value.AppId;
            var consumerSecret = _twitterConfig.Value.AppSecret;

            var accessTokenResponse = new UserModelDto();

            client.DefaultRequestHeaders.Accept.Clear();

            var oauthClient = new OAuthRequest
            {
                Method = "POST",
                Type = OAuthRequestType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                RequestUrl = "https://api.twitter.com/oauth/access_token",
                Token = token,
                Version = "1.0a",
                Realm = "twitter.com"
            };

            string auth = oauthClient.GetAuthorizationHeader();

            client.DefaultRequestHeaders.Add("Authorization", auth);


            try
            {
                var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("oauth_verifier", oauthVerifier)
            });

                using (var response = await client.PostAsync(oauthClient.RequestUrl, content))
                {
                    response.EnsureSuccessStatusCode();

                    //twiiter responds with a string concatenated by &
                    var responseString = response.Content.ReadAsStringAsync()
                                               .Result.Split("&");


                    //split by = to get actual values
                    accessTokenResponse = new UserModelDto
                    {
                        Token = responseString[0].Split("=")[1],
                        TokenSecret = responseString[1].Split("=")[1],
                        UserId = responseString[2].Split("=")[1],
                        Username = responseString[3].Split("=")[1]

                    };

                    var userAllInfo = new UserInfoModel
                    {
                        Token = responseString[0].Split("=")[1],
                        TokenSecret = responseString[1].Split("=")[1],
                        UserId = responseString[2].Split("=")[1],
                        Username = responseString[3].Split("=")[1]
                    };

                    var userInfo = context.TwitterUserInfo.FirstOrDefault(x => x.UserId == userAllInfo.UserId);
                    if (userInfo == null)
                    {
                        context.TwitterUserInfo.Add(userAllInfo);
                        context.SaveChanges();
                    }





                }
            }
            catch (Exception ex)
            {


            }

            return accessTokenResponse;
        }




        //get user information from token:-
    /*    public async Task<UserInfoResponse> GetUserInfo(string accessToken, string accessTokenSecret)
        {
            var client = _clientFactory.CreateClient("twitter");
            var consumerKey = _twitterConfig.Value.AppId;
            var consumerSecret = _twitterConfig.Value.AppSecret;

            client.DefaultRequestHeaders.Accept.Clear();

            var oauthClient = new OAuthRequest
            {
                Method = "GET",
                Type = OAuthRequestType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Token = accessToken,
                TokenSecret = accessTokenSecret,
                RequestUrl = "https://api.twitter.com/1.1/account/verify_credentials.json",
                Version = "1.0a",
                Realm = "twitter.com"
            };

            string auth = oauthClient.GetAuthorizationHeader();

            client.DefaultRequestHeaders.Add("Authorization", auth);

            try
            {
                using (var response = await client.GetAsync(oauthClient.RequestUrl))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        // Handle non-successful response (e.g., rate limiting or invalid tokens)
                        // Log the response content or error details for debugging
                        var errorContent = await response.Content.ReadAsStringAsync();
                        // Log errorContent or return a meaningful error response
                        throw new Exception($"Twitter API returned a non-successful status code: {response.StatusCode}. Error content: {errorContent}");
                    }

                    var userInfoJson = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfoResponse>(userInfoJson);
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                // Handle exceptions
                throw new Exception("An error occurred while fetching user information from Twitter.");
            }
        }

*/



    }
}
