namespace Jija.Models.Github
{
    public class OauthTokenResultDTO
    {
        public OauthTokenDTO Response {get; set;}
        public string ErrorMessage {get; set;}

        public OauthTokenResultDTO(OauthTokenDTO response)
        {
            Response = response;
        }

        public OauthTokenResultDTO(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}