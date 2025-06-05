using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Messages
{
    public class ErrorMessage : IMessage
    {
        public ErrorMessage(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
