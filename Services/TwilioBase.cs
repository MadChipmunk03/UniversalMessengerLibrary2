using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace UniversalMessengerLibrary2.Services
{
    public abstract class TwilioBase : IMessengerService<string, string>
    {
        public string Sender { get; set; }
        public List<string> Recipients { get; set; } = new List<string>();

        private string AccountSid { get; set; }
        private string AuthToken { get; set; }

        public TwilioBase(string accountSid, string authToken, string senderPhoneNumber)
        {
            AccountSid = accountSid;
            AuthToken = authToken;

            Sender = senderPhoneNumber;
        }

        public TwilioBase(string senderPhoneNumber)
        {
            AccountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            AuthToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

            Sender = senderPhoneNumber;
        }

        public virtual bool IsValid(string recipient)
        {
            return Regex.IsMatch(recipient, @"^\+?(\d| ){3,16}$");
        }

        public virtual void AddRecipient(string recipient)
        {
            if (!IsValid(recipient)) throw new Exception("Invalid phone number");
            Recipients.Add(recipient);
        }

        public abstract void FillRecipients();

        public void SendMessages(string message)
        {
            TwilioClient.Init(AccountSid, AuthToken);

            foreach (string recipient in Recipients)
            {
                // Find your Account SID and Auth Token at twilio.com/console
                // and set the environment variables. See http://twil.io/secure

                MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(Sender),
                    to: new Twilio.Types.PhoneNumber(recipient)
                );
            }
        }
    }
}
