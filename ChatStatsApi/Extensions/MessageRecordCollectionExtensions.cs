using ChatStatsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatStatsApi.Extensions
{
    public static class MessageRecordCollectionExtensions
    {


        public static IEnumerable<IGrouping<DateTime, MessageEntryTableEntity>> GroupByMonth(this IEnumerable<MessageEntryTableEntity> messages)
        {

            return messages.GroupBy(m => m.MessageTimestamp.ToFirstOfMonth());

            //throw new NotImplementedException(); 
        }

    }
}