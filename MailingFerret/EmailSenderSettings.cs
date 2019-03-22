using System;

namespace MailingFerret
{
    public class EmailSenderSettings
    {
        public readonly string MailHost;
        public readonly string MailUser;
        public readonly string MailPassword;
        public readonly string MailAccount;

        public EmailSenderSettings(string mailHost, string mailUser, string mailPassword, string mailAccount)
        {
            if (string.IsNullOrWhiteSpace(mailHost)) throw new ArgumentException(nameof(mailHost));
            MailHost = mailAccount;
            if (string.IsNullOrWhiteSpace(mailUser)) throw new ArgumentException(nameof(mailUser));
            MailUser = mailUser;
            if (string.IsNullOrWhiteSpace(mailPassword)) throw new ArgumentException(nameof(mailPassword));
            MailPassword = mailPassword;
            if (string.IsNullOrWhiteSpace(mailAccount)) throw new ArgumentException(nameof(mailAccount));
            MailAccount = mailAccount;
        }
    }
}