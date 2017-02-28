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
        private readonly StorageFactory storageFactory;

        public DataUploadController(StorageFactory storageFactory)
        {
            this.storageFactory = storageFactory;
        }

        private IHttpActionResult Hangouts(HttpPostedFileBase file)
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

                var messagesTableClient = storageFactory.CreateTableStorageClient<MessageEntryTableEntity>(MessageEntryTableEntity.MessageTableName);

                messagesTableClient.InsertOrUpdate(regularMessages.ToArray());
                //tableStorageClient.Insert(otherMessages.ToArray());

            }



            return Ok();

        }

        public async Task<IHttpActionResult> Hangouts()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsStreamAsync();

                var csvReader = new StreamReader(buffer);

                csvReader.ReadLine(); //read the header
                var regularMessages = new List<MessageEntryTableEntity>();
                var otherMessages = new List<MessageEntryTableEntity>();
                ReadMessages(csvReader, regularMessages, otherMessages);

                csvReader.Close();
                csvReader.Dispose();
                //Do whatever you want with filename and its binaray data.

                var messagesTableClient = storageFactory.CreateTableStorageClient<MessageEntryTableEntity>(MessageEntryTableEntity.MessageTableName);

                messagesTableClient.InsertOrUpdate(regularMessages.ToArray());
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
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
