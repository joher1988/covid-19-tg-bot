using System;
using System.Linq;
using System.Threading.Tasks;

namespace COVID19.Termin.Bot.Commands
{
    public class StartCommand:IBotCommand
    {
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly ICheckService _checkService;

        public StartCommand(ISubscriptionManager subscriptionManager, ICheckService checkService)
        {
            _subscriptionManager = subscriptionManager;
            _checkService = checkService;
        }
        public string Command => "start";
        public string Description => "start tracking";
        public bool InternalCommand => false;
        public async Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText)
        {
            await _subscriptionManager.Add(chatId.ToString(), async () =>
            {
                var result = await _checkService.CheckTermin();
                foreach (var r in result)
                {
                    await chatService.SendMessage(chatId, r);
                }
                    
            });
        }
    }
    public class StopCommand : IBotCommand
    {
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly ICheckService _checkService;

        public StopCommand(ISubscriptionManager subscriptionManager, ICheckService checkService)
        {
            _subscriptionManager = subscriptionManager;
            _checkService = checkService;
        }
        public string Command => "stop";
        public string Description => "stop tracking";
        public bool InternalCommand => false;
        public async Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText)
        {
            await _subscriptionManager.Remove(chatId.ToString());
        }
    }
    public class CheckCommand : IBotCommand
    {
        private readonly ICheckService _checkService;

        public CheckCommand( ICheckService checkService)
        {
            _checkService = checkService;
        }
        public string Command => "check";
        public string Description => "check";
        public bool InternalCommand => false;
        public async Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText)
        {
            var result = await _checkService.CheckTermin();
            if(result.Count() == 0)
                await chatService.SendMessage(chatId, "Нет терминов =(");
            foreach (var r in result)
            {
                await chatService.SendMessage(chatId, r);
            }
        }
    }
}