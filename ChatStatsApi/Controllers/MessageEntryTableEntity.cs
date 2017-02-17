using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatStatsApi.Controllers
{
    public class MessageEntryTableEntity : TableEntity
    {

        public string Sender { get; set; }
        public int UnixTime { get; set; }
        public string Message { get; set; }
        public DateTime MessageTimestamp { get; set; }
        public string MessageType { get; set; }

        public MessageEntryTableEntity()
        {

        }


        public MessageEntryTableEntity(string sender, int unixTime, string message, DateTime messageTimestamp)
        {
            this.PartitionKey = sender;
            this.RowKey = unixTime.ToString();
            this.Sender = sender;
            this.UnixTime = unixTime;
            this.Message = message;
            this.MessageTimestamp = MessageTimestamp;
        }


    }
}