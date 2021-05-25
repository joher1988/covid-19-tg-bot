using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace COVID19.Termin.Bot
{
    public class SubscribtionTriggerHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ISubscriptionManager _subscriptionManager;
        private Timer _timer;

        public SubscribtionTriggerHostedService(IServiceProvider services, ISubscriptionManager subscriptionManager)
        {
            _services = services;
            _subscriptionManager = subscriptionManager;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(async state => await DoWork(stoppingToken),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMilliseconds(-1));

            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                using var scope = _services.CreateScope();
                _subscriptionManager.Run();
            }
            finally
            {
                _timer?.Change
                (TimeSpan.FromMinutes(10),
                    TimeSpan.FromMilliseconds(-1));
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
            }

            base.Dispose();
        }
    }
}
