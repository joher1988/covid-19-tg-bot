using System;
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
                await chatService.SendMessage(chatId, result ? "Есть термин !!!!!!" : "Нет терминов =(");
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
            await chatService.SendMessage(chatId, result ? "Есть термин !!!!!!" : "Нет терминов =(");
        }
    }
}