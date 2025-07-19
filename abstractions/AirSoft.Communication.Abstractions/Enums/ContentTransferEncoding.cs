namespace AirSoft.Communication.Abstractions.Enums
{
    /// <summary>
    /// Specifies the content transfer encoding methods for MIME entities according to RFC 2045
    /// </summary>
    /// <remarks>
    /// <para>
    /// Defines how binary data should be encoded for transmission over SMTP (7-bit channels).
    /// </para>
    /// <para>
    /// Reference: RFC 2045 Section 6 (Content-Transfer-Encoding header field)
    /// </para>
    /// </remarks>
    public enum ContentTransferEncoding
    {
        /// <summary>
        /// 7-bit ASCII (no encoding). Suitable for short text messages.
        /// </summary>
        SevenBit,

        /// <summary>
        /// 8-bit character encoding. Used for extended ASCII characters.
        /// </summary>
        EightBit,

        /// <summary>
        /// Raw binary encoding (should only be used over 8-bit clean transports)
        /// </summary>
        Binary,

        /// <summary>
        /// Base64 encoding (most common for binary attachments)
        /// </summary>
        Base64,

        /// <summary>
        /// Quoted-Printable encoding (used for text with special characters)
        /// </summary>
        QuotedPrintable
    }
}
