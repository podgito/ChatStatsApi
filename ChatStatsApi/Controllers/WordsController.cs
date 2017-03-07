using ChatStatsApi.AzureStorage;
using ChatStatsApi.Models;
using System;
using System.Web.Http;
using ChatStatsApi.Extensions;
using System.Collections.Generic;
using System.Linq;
using ChatStatsApi.Data;

namespace ChatStatsApi.Controllers
{
    [RoutePrefix("api/word")]
    public class WordsController : ApiController
    {
        private readonly IMessageRepository messageRepository;
        private readonly WordsDataRepository wordsDataRepo;

        public WordsController(IMessageRepository messageRepository, WordsDataRepository wordsDataRepo)
        {
            this.messageRepository = messageRepository;
            this.wordsDataRepo = wordsDataRepo;
        }

        [Route("{word}/count/monthly")]
        public IEnumerable<object> GetMonthlyCountsForWord(string word)
        {
            var wordMessages = messageRepository.GetMessagesContainingWord(word);

            var groupedMessages = wordMessages.GroupByMonth();

            var monthlyCounts = groupedMessages.Select(c => new MessageIntervalCounts { Date = c.Key, Count = c.Count() });

            return monthlyCounts.Select(c => new { x = c.Date.ToJsUtcDateMilliseconds(), c.Count });
        }

        [Route("counts")]
        public IEnumerable<object> GetWordCounts()
        {
            var distinctWords = messageRepository.GetAllWords().Where(w => !wordsDataRepo.GetInsignificantWords().Contains(w));

            var wordCounts = distinctWords.GroupBy(c => c).Select(g => new { Word = g.Key, Count = g.Count() });

            return wordCounts.OrderByDescending(a => a.Count).Take(100);
        }


        [Route("Hashtags/counts")]
        public IEnumerable<object> GetHashTagCounts()
        {
            var distinctHashTags = messageRepository.GetAllWords().Where(w => w.StartsWith("#"));

            var wordCounts = distinctHashTags.GroupBy(c => c).Select(g => new { Word = g.Key, Count = g.Count() });

            return wordCounts.OrderByDescending(a => a.Count);
        }

        //Wordcloud

        //Weekly/monthly counts of individual words
    }
}