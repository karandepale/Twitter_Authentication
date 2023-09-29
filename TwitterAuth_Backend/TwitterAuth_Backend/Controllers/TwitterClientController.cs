using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TwitterAuth_Backend.Data;

namespace TwitterAuth_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterClientController : ControllerBase
    {
        private readonly ITwitterAuthRepository _twitterAuth;
        private readonly IConfiguration _config;

        public TwitterClientController(
            ITwitterAuthRepository twitterAuth,
            IConfiguration config
            )
        {
            _config = config;
            _twitterAuth = twitterAuth;
        }

        [HttpGet("GetRequestToken")]
        public async Task<IActionResult> GetRequestToken()
        {
            //STEP 1 call made to /oauth/request_token
            var response = await _twitterAuth.GetRequestToken();

            return Ok(response);

        }







        [HttpGet("sign-in-with-twitter")]
        public async Task<IActionResult> SignInWithTwitter(string oauth_token, string oauth_verifier)
        {

            var response = await _twitterAuth.GetAccessToken(oauth_token, oauth_verifier);


            return Ok(response);

        }


        /*  [HttpGet("GetUserInfo")]
          public async Task<IActionResult> GetUserInfo(string accessToken, string accessTokenSecret)
          {
              try
              {
                  // Call the GetUserInfo method in your repository to fetch user information
                  var userInfo = await _twitterAuth.GetUserInfo(accessToken, accessTokenSecret);

                  if (userInfo != null)
                  {
                      return Ok(userInfo); // Return user information as JSON
                  }
                  else
                  {
                      return NotFound(); // User not found
                  }
              }
              catch (Exception ex)
              {
                  // Handle exceptions

                  // Log the exception for debugging purposes
                  Console.WriteLine(ex.Message);
                  Console.WriteLine(ex.StackTrace);

                  // Handle exceptions
                  return StatusCode(StatusCodes.Status500InternalServerError, " Karan---Internal server error");
              }
          }
      }*/


    }

}