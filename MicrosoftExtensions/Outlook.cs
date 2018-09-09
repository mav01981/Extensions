namespace MicrosoftExtensions
{
    using Microsoft.Office.Interop.Outlook;

    public static class OutlookExtensions
    {
        public static string FormatEmailAddress(this MailItem mail)
        {
            AddressEntry sender = mail.Sender;
            string SenderEmailAddress = string.Empty;

            if (sender.AddressEntryUserType == OlAddressEntryUserType.olExchangeUserAddressEntry || sender.AddressEntryUserType == OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
            {
                ExchangeUser exchUser = sender.GetExchangeUser();
                if (exchUser != null)
                {
                    SenderEmailAddress = exchUser.PrimarySmtpAddress;
                }
            }
            else
            {
                SenderEmailAddress = mail.SenderEmailAddress;
            }

            return SenderEmailAddress;
        }
    }
}
