using ChatStatsApi.Models;
using Pojito.Azure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ChatStatsApi.Controllers
{
    /// <summary>
    /// Apis to handle the upload and storage of data
    /// </summary>
    public class DataUploadController : ApiController
    {
        private const string RegularMessageType = "REGULAR_CHAT_MESSAGE";
        private readonly TableStorageClient<MessageEntryTableEntity> tableStorageClient;

        public DataUploadController(TableStorageClient<MessageEntryTableEntity> tableStorageClient)
        {
            this.tableStorageClient = tableStorageClient;
        }

        public IHttpActionResult Hangouts(HttpPostedFileBase file)
        {
            //Store the data in table storage
            if (file != null && file.ContentLength > 0)
            {
                var csvReader = new StreamReader(file.InputStream);

                csvReader.ReadLine(); //read the header
                var regularMessages = new List<MessageEntryTableEntity>();
                var otherMessages = new List<MessageEntryTableEntity>();
                ReadMessages(csvReader, regularMessages, otherMessages);

                csvReader.Close();
                csvReader.Dispose();

                tableStorageClient.Insert(regularMessages.ToArray());
                tableStorageClient.Insert(otherMessages.ToArray());

            }



            return Ok();

        }

        private static void ReadMessages(StreamReader csvReader, List<MessageEntryTableEntity> regularMessages, List<MessageEntryTableEntity> otherMessages)
        {
            while (!csvReader.EndOfStream)
            {
                var line = csvReader.ReadLine();

                try
                {
                    var message = MessageEntryTableEntity.RecordFromCsvString(line);
                    if (message.MessageType == RegularMessageType)
                    {
                        regularMessages.Add(message);
                    }
                    else
                    {
                        otherMessages.Add(message);
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }
    }
}
