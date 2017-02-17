using Pojito.Azure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChatStatsApi.Controllers
{
    /// <summary>
    /// Apis to handle the upload and storage of data
    /// </summary>
    public class DataUploadController : ApiController
    {
        private readonly TableStorageClient<MessageEntryTableEntity> tableStorageClient;

        public DataUploadController(TableStorageClient<MessageEntryTableEntity>  tableStorageClient)
        {
            this.tableStorageClient = tableStorageClient;
        }

        public async Task<IHttpActionResult> Hangouts(object data)
        {
            //Store the data in table storage
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                //Do whatever you want with filename and its binaray data.
            }

            return Ok();

        }

    }
}
