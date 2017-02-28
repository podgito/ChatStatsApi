using ChatStatsApi.Models;
using Pojito.Azure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ChatStatsApi.AzureStorage
{
    public class MessageRepository : IMessageRepository
    {
        private readonly StorageFactory storageFactory;

        public MessageRepository(StorageFactory storageFactory)
        {
            this.storageFactory = storageFactory;
        }

        public IEnumerable<string> DistinctWords()
        {
            var words = new List<string>();
            var messageClient = storageFactory.CreateTableStorageClient<MessageEntryTableEntity>(MessageEntryTableEntity.MessageTableName);
            var allMessages = messageClient.GetAll().Select(m => m.Message);

            foreach(var message in allMessages)
            {
                var messageWords = message.Split(new char[] { ' ', '-', ',', '.' },  StringSplitOptions.RemoveEmptyEntries).Distinct();
                words.AddRange(messageWords);
            }

            return words;
        }

        public IEnumerable<string> GetAllWords()
        {
            var words = new List<string>();
            var messageClient = storageFactory.CreateTableStorageClient<MessageEntryTableEntity>(MessageEntryTableEntity.MessageTableName);
            var allMessages = messageClient.GetAll().Select(m => m.Message);
            

            foreach (var message in allMessages)
            {
                var messageWords = message.Split(new char[] { ' ', '-', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
                words.AddRange(messageWords);
            }

            return words;
        }

        public IEnumerable<MessageEntryTableEntity> GetMessagesContainingWord(string word)
        {
            var messageClient = storageFactory.CreateTableStorageClient<MessageEntryTableEntity>(MessageEntryTableEntity.MessageTableName);

            var allMessages = messageClient.GetAll();

            //Perhaps look for the word string as a word with spaces either side?????

            return allMessages.Where(m => m.Message.ToUpper().Contains(word.ToUpper()));
        }

        public void Insert(IEnumerable<MessageEntryTableEntity> messages)
        {
            throw new NotImplementedException();
        }
    }
}