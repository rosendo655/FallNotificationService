using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallNotificationService
{
    public enum ChatStatus
    {
        NEW,
        ASK_REGISTER,
        USER_EXISTS,
    }
    public class ChatSession
    {
        private static int MAX_MESSAGE_COUNT = 5;
        public ChatSession(int ChatId, string username)
        {
            this.ChatId = ChatId;
            Username = username;
            Status = ChatStatus.NEW;
            PreviousStatus = ChatStatus.NEW;
            PreviousMessages = new Queue<string>();
        }


        public int ChatId { get; set; }
        public string Username { get; set; }
        public ChatStatus PreviousStatus { get; set; }

        private ChatStatus _status;
        public ChatStatus Status
        {
            get => _status;
            set
            {
                PreviousStatus = _status;
                _status = value;
            }
        }

        public Queue<string> PreviousMessages { get; }
        public string CurrentMessage => PreviousMessages?.LastOrDefault();

        public void AddMessage(string message)
        {
            if (PreviousMessages.Count >= MAX_MESSAGE_COUNT)
            {
                PreviousMessages.Dequeue();
            }
            PreviousMessages.Enqueue(message);
        }
    }
}
