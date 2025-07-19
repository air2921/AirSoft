namespace AirSoft.Communication.Abstractions.Enums
{
    /// <summary>
    /// Specifies the priority of an email message according to RFC 5322 (Section 3.6.4) and RFC 4021
    /// </summary>
    /// <remarks>
    /// <para>
    /// The numeric values correspond to standard SMTP priorities:
    /// <list type="bullet">
    /// <item><description>High (1) - Urgent messages</description></item>
    /// <item><description>Normal (3) - Regular priority (default)</description></item>
    /// <item><description>Low (5) - Non-urgent messages</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// These values map to the X-Priority header used by many email clients:
    /// <list type="table">
    /// <listheader>
    /// <term>Priority</term>
    /// <term>X-Priority Value</term>
    /// </listheader>
    /// <item><term>High</term><term>1 (Highest)</term></item>
    /// <item><term>Normal</term><term>3 (Normal)</term></item>
    /// <item><term>Low</term><term>5 (Lowest)</term></item>
    /// </list>
    /// </para>
    /// </remarks>
    public enum MailPriority
    {
        /// <summary>
        /// High priority message (urgent/important)
        /// </summary>
        High = 1,

        /// <summary>
        /// Normal priority message (default)
        /// </summary>
        Normal = 3,

        /// <summary>
        /// Low priority message (non-urgent)
        /// </summary>
        Low = 5
    }
}
