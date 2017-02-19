using ChatStatsApi.Models;
using Pojito.Azure.Storage.Table;
using System.Collections.Generic;
using System.Linq;

namespace ChatStatsApi.AzureStorage
{
    public class MessageRepository : IMessageRepository
    {
        private readonly StorageFactory storageFactory;

        public MessageRepository(StorageFactory storageFactory)
        {
            this.storageFactory = storageFactory;
        }

        public IEnumerable<MessageEntryTableEntity> GetMessagesContainingWord(string word)
        {
            var messageClient = storageFactory.CreateTableStorageClient<MessageEntryTableEntity>(MessageEntryTableEntity.MessageTableName);

            var allMessages = messageClient.GetAll();

            //Perhaps look for the word string as a word with spaces either side?????

            return allMessages.Where(m => m.Message.Contains(word));
        }
    }
}