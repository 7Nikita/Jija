namespace Jija.Models.Github
{
    public class UserInfoResultDTO
    {
        public UserInfoDTO Response { get; set; }
        public string ErrorMessage { get; set; }

        public UserInfoResultDTO(UserInfoDTO userInfoDto)
        {
            Response = userInfoDto;
        }

        public UserInfoResultDTO(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}