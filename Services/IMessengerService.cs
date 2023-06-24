using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalMessengerLibrary2.Services
{
    internal interface IMessengerService<T, U>
    {
        T Sender { get; set; }
        List<T> Recipients { get; set; }

        bool IsValid(T recipient);
        void AddRecipient(T recipient);
        void FillRecipients();
        void SendMessages(U message);
    }
}
