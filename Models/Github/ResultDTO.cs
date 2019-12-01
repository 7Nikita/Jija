namespace Jija.Models.Github
{
    public class ResultDTO<T>
    {
        public T Response { get; set; }
        public string ErrorMessage { get; set; }
        
        public ResultDTO(T resultInfoDTO)
        {
            Response = resultInfoDTO;
        }

        public ResultDTO(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}