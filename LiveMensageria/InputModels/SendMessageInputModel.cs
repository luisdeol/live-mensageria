using System;

namespace LiveMensageria.InputModels
{
    public class SendMessageInputModel
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
