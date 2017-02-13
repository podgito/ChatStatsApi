using ChatStatsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatStatsApi.Controllers
{
    [RoutePrefix("api/word")]
    public class WordsController : ApiController
    {

        [Route("{word}/count/monthly")]
        public WordCloudModel GetMonthlyCountsForWord(string word)
        {
            throw new NotImplementedException();
        }

    }
}
