using ChatStatsApi.Models;
using Pojito.Azure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatStatsApi.Controllers
{
    [RoutePrefix("api/word")]
    public class WordsController : ApiController
    {
        private readonly StorageFactory storageFactory;

        public WordsController(StorageFactory storageFactory)
        {
            this.storageFactory = storageFactory;
        }

        [Route("{word}/count/monthly")]
        public WordCloudModel GetMonthlyCountsForWord(string word)
        {
            var repo = storageFactory.CreateTableStorageClient<MessageEntryTableEntity>(MessageEntryTableEntity.MessageTableName);

            var messages = repo.GetMessages(); 

            throw new NotImplementedException();
        }

    }
}
