using Pojito.Azure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatStatsApi.AzureStorage
{
    public class MessageRepository : IMessageRepository
    {
        private readonly StorageFactory storageFactory;

        public MessageRepository(StorageFactory storageFactory)
        {
            this.storageFactory = storageFactory;
        }


        public IEnumerable<string> GetMessagesContainingWord(string word)
        {
            throw new NotImplementedException();
        }
    }
}