using ChatStatsApi.AzureStorage;
using ChatStatsApi.Models;
using System;
using System.Web.Http;
using ChatStatsApi.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ChatStatsApi.Controllers
{
    [RoutePrefix("api/word")]
    public class WordsController : ApiController
    {
        private readonly IMessageRepository messageRepository;

        public WordsController(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        [Route("{word}/count/monthly")]
        public IEnumerable<MessageIntervalCounts> GetMonthlyCountsForWord(string word)
        {
            var wordMessages = messageRepository.GetMessagesContainingWord(word);

            var groupedMessages = wordMessages.GroupByMonth();

            return groupedMessages.Select(c => new MessageIntervalCounts { Date = c.Key, Count = c.Count() });
        }

        //Wordcloud

        //Weekly/monthly counts of individual words
    }
}