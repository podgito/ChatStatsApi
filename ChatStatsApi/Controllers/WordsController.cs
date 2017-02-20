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
        public IEnumerable<object> GetMonthlyCountsForWord(string word)
        {
            var wordMessages = messageRepository.GetMessagesContainingWord(word);

            var groupedMessages = wordMessages.GroupByMonth();

            var monthlyCounts = groupedMessages.Select(c => new MessageIntervalCounts { Date = c.Key, Count = c.Count() });

            return monthlyCounts.Select(c => new { x = c.Date.ToJsUtcDateMilliseconds(), c.Count });
        }


        public IEnumerable<object> GetWordCounts()
        {
            var distinctWords = messageRepository.GetAllWords();

            var wordCounts = distinctWords.GroupBy(c => c).Select(g=> new { x = g.Key, y = g.Count() });

            return wordCounts;
        }

        //Wordcloud

        //Weekly/monthly counts of individual words
    }
}