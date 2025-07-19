namespace AirSoft.Communication.Abstractions
{
    /// <summary>
    /// Base interface for messages (email, SMS, etc.)
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Recipient address (email, phone number, etc.)
        /// </summary>
        string To { get; set; }
    }
}
