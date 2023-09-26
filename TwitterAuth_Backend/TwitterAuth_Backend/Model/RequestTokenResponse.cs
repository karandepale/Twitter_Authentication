namespace TwitterAuth_Backend.Model
{
    public class RequestTokenResponse
    {
        public string oauth_token { get; set; }
        public string oauth_token_secret { get; set; }
        public string oauth_callback_confirmed { get; set; }
    }
}
