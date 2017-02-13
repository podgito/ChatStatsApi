using System.Collections.Generic;

namespace ChatStatsApi.Models
{
    public class WordCloudModel
    {
        protected List<WordCount> WordCounts { get; set; }

        public WordCloudModel(List<WordCount> wordCounts)
        {
            this.WordCounts = wordCounts;
        }

        public class WordCount
        {
            public string Word { get; set; }
            public int Count { get; set; }
        }

        /// <summary>
        /// Creates the json output required for the wordcloud library e.g. [["word1", 123],["word2", 98]]
        /// </summary>
        /// <returns></returns>
        public List<object[]> JsonOutput
        {
            get
            {
                var data = new List<object[]>();
                foreach (var wordCount in WordCounts)
                {
                    var item = new object[] { wordCount.Word, wordCount.Count };
                    data.Add(item);
                }
                return data;
            }
        }


    }
}