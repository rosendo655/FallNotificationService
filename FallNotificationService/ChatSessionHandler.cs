using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FallNotificationService
{
    public class ChatSessionHandler
    {
        private static Dictionary<ChatStatus, Func<ChatSession, Task>> handleDictionary = new Dictionary<ChatStatus, Func<ChatSession, Task>>()
        {
            {ChatStatus.NEW , NewUser },
            {ChatStatus.ASK_REGISTER , AskRegister },
            {ChatStatus.USER_EXISTS , UserExists }
        };

        private const string YES = "Si";
        private const string NO = "No";
        private static IKeyboard YesNoKeyboard => ReplyKeyboard.FromStringKeys(new[] { new[] { YES, NO } });


        public static async Task HandleSession(ChatSession session)
        {
            var taskToDo = handleDictionary[session.Status];

            await taskToDo(session);
        }


        private static async Task NewUser(ChatSession session)
        {
            await TelegramBotManager.SendMessage(session.ChatId, $"Hola {session.Username}, deseas registrarte?", YesNoKeyboard);
            session.Status = ChatStatus.ASK_REGISTER;
        }

        private static async Task AskRegister(ChatSession session)
        {
            if (session.CurrentMessage == YES)
            {
                await TelegramBotManager.SendMessage(session.ChatId, $"De acuerdo {session.Username}, Ya has sido registrado.");

                session.Status = ChatStatus.USER_EXISTS;
            }
            else
            {
                session.Status = ChatStatus.NEW;
            }
        }

        private static async Task UserExists(ChatSession session)
        {
            await TelegramBotManager.SendMessage(session.ChatId, $"Hola {session.Username}, Ya has sido registrado.");
        }
    }
}
