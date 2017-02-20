using ChatStatsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStatsApi.AzureStorage
{
    public interface IMessageRepository
    {

        IEnumerable<MessageEntryTableEntity> GetMessagesContainingWord(string word);

        IEnumerable<string> DistinctWords();

        IEnumerable<string> GetAllWords();

    }
}
