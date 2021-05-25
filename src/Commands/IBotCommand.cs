using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVID19.Termin.Bot.Commands
{
    public interface IBotCommand
    {
        string Command { get; }
        string Description { get; }
        bool InternalCommand { get; }

        Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText);
    }
}
