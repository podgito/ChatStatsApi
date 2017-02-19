using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatStatsApi.Models
{
    public class MessageEntryTableEntity : TableEntity
    {

        public const string OtherMessageTypesTableName = "OtherMessages";
        public const string MessageTableName = "RegularMessages";

        public string Sender { get; set; }
        public Int64 UnixTime { get; set; }
        public string Message { get; set; }
        public DateTime MessageTimestamp { get; set; }
        public string MessageType { get; set; }

        public MessageEntryTableEntity()
        {

        }

        public static MessageEntryTableEntity RecordFromCsvString(string recordString)
        {

            var parameters = recordString.Split(new string[] { ";" }, StringSplitOptions.None);

            var unixTime = Convert.ToInt64(parameters[0].Replace("\"", ""));
            var timeStamp = Convert.ToDateTime(parameters[1].Replace("\"", ""));
            var sender = Cleanup(parameters[3]);
            var messageType = Cleanup(parameters[4]);
            var message = Cleanup(parameters[5]);

            return new MessageEntryTableEntity(sender, unixTime, message, timeStamp, messageType);

        }

        private static string Cleanup(string input)
        {
            return input.Replace("\"", "");
        }




        public MessageEntryTableEntity(string sender, Int64 unixTime, string message, DateTime messageTimestamp, string messageType)
        {
            this.PartitionKey = sender;
            this.RowKey = unixTime.ToString();
            this.Sender = sender;
            this.UnixTime = unixTime;
            this.Message = message;
            this.MessageTimestamp = messageTimestamp;
            this.MessageType = messageType;
        }


    }
}