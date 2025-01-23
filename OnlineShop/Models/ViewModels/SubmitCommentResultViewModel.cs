namespace OnlineShop.Models.ViewModels
{
    public class SubmitCommentResultViewModel
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public SubmitCommentResultViewModel(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
