using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ChatStatsApi.Data
{
    public class WordsDataRepository
    {


        public IEnumerable<string> GetInsignificantWords()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ChatStatsApi.Data.WordsData.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<WordsData>(result).InsignificantWords;
            }

        }

        class WordsData
        {
            public IEnumerable<string> InsignificantWords { get; set; }
        }
    }
}