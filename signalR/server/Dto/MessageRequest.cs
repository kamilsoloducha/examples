using System.Collections.Generic;

namespace Server.Dto
{
    public class MessageRequest
    {
        public string From { get; set; }
        public IReadOnlyCollection<string> To { get; set; }
        public string Message { get; set; }

    }
}
