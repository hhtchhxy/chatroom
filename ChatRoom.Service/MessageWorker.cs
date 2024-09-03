using ChatRoom.Core.Cache;
using ChatRoom.Model.Models.Chat;
using ChatRoom.Repository.Chat.IRepository;
using ChatRoom.Repository.Chat.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom.Service
{
    public class MessageWorker : IHostedService
    {
        private readonly IMessageRepositoryService _messageRepositoryService;
        private readonly ILogger<MessageWorker> _logger;

        public MessageWorker(IMessageRepositoryService messageRepositoryService ,ILogger<MessageWorker> logger)
        {
            this._messageRepositoryService = messageRepositoryService;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            int batchNum = 100;
            return Task.Factory.StartNew(async () => {
                _logger.LogInformation($"MessageWorker开始执行 - {DateTime.Now}");
                while (!cancellationToken.IsCancellationRequested)
                {
                   
                    List<MessageInfoDO> messages = new List<MessageInfoDO>();
                    try
                    {
                        int queueLen = LocalCacheHelper.MsgQueue.Count;
                        if (queueLen > 0)
                        {
                            _logger.LogInformation($"MessageWorker.msgQueue - {queueLen}");
                            int listCount = 0;
                            while(LocalCacheHelper.MsgQueue.TryDequeue(out MessageInfoDO info))
                            {
                                messages.Add(info); 
                                if (messages.Count>= batchNum|| messages.Count==queueLen)
                                {
                                    _messageRepositoryService.DoBulk(messages);
                                    messages.Clear();
                                }
                            }
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                        }
                    }
                    catch (OperationCanceledException) {
                    }
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
