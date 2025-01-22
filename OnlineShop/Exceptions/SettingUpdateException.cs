namespace OnlineShop.Exceptions
{
    public class SettingUpdateException : Exception
    {
        public SettingUpdateException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
